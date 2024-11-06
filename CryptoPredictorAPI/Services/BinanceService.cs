using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;
using System.Globalization;
using System.Text.Json;

namespace CryptoPredictorAPI.Services;

public class BinanceService : IBinanceService
{
    private readonly HttpClient _httpClient;
    private readonly IBinanceHttpRequestMessageCreator _messageCreator;
    private readonly IBinanceResponseHandler _responseHandler;
    private readonly IBinanceJsonDeserializer _jsonDeserializer;
    private readonly BinanceDbContext _dbContext;
    private readonly IBinanceDataConverter _dataConverter;

    public BinanceService(
        IHttpClientFactory httpClientFactory,
        IBinanceHttpRequestMessageCreator messageCreator,
        IBinanceResponseHandler responseHandler,
        IBinanceJsonDeserializer jsonDeserializer,
        BinanceDbContext dbContext,
        IBinanceDataConverter dataConverter)
    {
        _httpClient = httpClientFactory.CreateClient("BinanceClient");
        _messageCreator = messageCreator;
        _responseHandler = responseHandler;
        _jsonDeserializer = jsonDeserializer;
        _dbContext = dbContext;
        _dataConverter = dataConverter;
    }


    public async Task<decimal?> FetchPrice(string symbol)
    {
        var request = _messageCreator.CreatePriceRequestMessage(symbol);
        var response = await _httpClient.SendAsync(request);
        var responseData = await _responseHandler.HandleResponse(response);

        var priceData = _jsonDeserializer.Deserialize<BinancePriceDataModel>(responseData);
        if (priceData != null)
        {
            if (decimal.TryParse(priceData.Price, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out var priceValue))
                return priceValue;
        }
        return null;
    }

    public async Task<List<List<object>>> FetchHistoricalData(string symbol, string interval, int limit = 500)
    {
        var request = _messageCreator.CreateHistoricalDataRequestMessage(symbol, interval, limit);
        var response = await _httpClient.SendAsync(request);
        var responseData = await _responseHandler.HandleResponse(response);

        return _jsonDeserializer.Deserialize<List<List<object>>>(responseData);
    }
    public async Task<List<List<object>>> FetchHistoricalData24h(string symbol)
    {
        const string interval = "1h";
        const int limit = 24;

        var request = _messageCreator.CreateHistoricalKlinesRequestMessage(symbol, interval, limit);
        var response = await _httpClient.SendAsync(request);
        var responseData = await _responseHandler.HandleResponse(response);

        return _jsonDeserializer.Deserialize<List<List<object>>>(responseData);
    }

    public async Task<List<BinanceKlineModel>> FetchAllHistoricalData(string symbol, long? startTime = null, long? endTime = null)
    {
        const string interval = "1d";
        const int limit = 1000;

        var request = _messageCreator.CreateHistoricalKlinesRequestMessage(symbol, interval, limit, startTime, endTime);
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error getting data from Binance.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
        {
            var rootElement = doc.RootElement;
            if (rootElement.ValueKind == JsonValueKind.Array)
            {
                var models = _dataConverter.ConvertKlineData(rootElement.EnumerateArray().ToList());
                models = models.OrderBy(m => m.OpenTime).ToList();
                _dbContext.BinanceHistoricalData.AddRange(models);
                await _dbContext.SaveChangesAsync();
                return models;
            }
            else
            {
                throw new Exception("Invalid JSON structure.");
            }
        }
    }
}
