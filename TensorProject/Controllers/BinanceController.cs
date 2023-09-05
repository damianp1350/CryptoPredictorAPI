using Microsoft.AspNetCore.Mvc;
using TensorProject.Models;
using TensorProject.Services.IServices;
using System.Globalization;
using TensorProject.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TensorProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceDataConverter _dataConverter;
        private readonly IBinanceHttpRequestMessageCreator _messageCreator;
        private readonly IBinanceResponseHandler _responseHandler;
        private readonly IBinanceJsonDeserializer _jsonDeserializer;
        private readonly HttpClient _httpClient;
        private readonly BinanceDbContext _dbContext;

        public BinanceController(
            IBinanceHttpRequestMessageCreator messageCreator,
            IBinanceResponseHandler responseHandler,
            IBinanceJsonDeserializer jsonDeserializer,
            HttpClient httpClient,
            BinanceDbContext context,
            IBinanceDataConverter dataConverter)
        {
            _messageCreator = messageCreator;
            _responseHandler = responseHandler;
            _jsonDeserializer = jsonDeserializer;
            _httpClient = httpClient;
            _dbContext = context;
            _dataConverter = dataConverter;
        }


        [HttpGet("price/{symbol}")]
        public async Task<IActionResult> GetPrice(string symbol)
        {
            var request = _messageCreator.CreatePriceRequestMessage(symbol);
            var response = await _httpClient.SendAsync(request);
            var responseData = await _responseHandler.HandleResponse(response);

            var priceData = _jsonDeserializer.Deserialize<BinancePriceDataModel>(responseData);
            if (priceData != null)
            {
                if (Decimal.TryParse(priceData.Price, NumberStyles.Number, new CultureInfo("en-US"), out var priceValue))
                    return Ok(priceValue);
                else
                    return BadRequest();
            }

            return NotFound();
        }

        [HttpGet("historical-data/{symbol}/{interval}")]
        public async Task<IActionResult> GetHistoricalData(string symbol, string interval, int limit = 500)
        {
            var request = _messageCreator.CreateHistoricalDataRequestMessage(symbol, interval, limit);
            var response = await _httpClient.SendAsync(request);
            var responseData = await _responseHandler.HandleResponse(response);

            var historicalData = _jsonDeserializer.Deserialize<List<List<object>>>(responseData);
            if (historicalData != null && historicalData.Any())
                return Ok(historicalData);

            return NotFound();
        }

        [HttpGet("last24hours/{symbol}")]
        public async Task<IActionResult> GetHistoricalData24h(string symbol)
        {
            const string interval = "1h";
            const int limit = 24;

            var request = _messageCreator.CreateHistoricalKlinesRequestMessage(symbol, interval, limit);
            var response = await _httpClient.SendAsync(request);
            var responseData = await _responseHandler.HandleResponse(response);

            var historicalData = _jsonDeserializer.Deserialize<List<List<object>>>(responseData);
            if (historicalData != null && historicalData.Any())
                return Ok(historicalData);

            return NotFound();
        }

        [HttpGet("allHistoricalData/{symbol}")]
        public async Task<IActionResult> GetAllHistoricalData(string symbol, long? startTime = null, long? endTime = null)
        {
            const string interval = "1d";
            const int limit = 1000;

            var request = _messageCreator.CreateHistoricalKlinesRequestMessage(symbol, interval, limit, startTime, endTime);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error getting data from Binance.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                var rootElement = doc.RootElement;
                if (rootElement.ValueKind == JsonValueKind.Array)
                {
                    var models = _dataConverter.ConvertKlineData(rootElement.EnumerateArray().ToList());

                    _dbContext.BinanceHistoricalDatas.AddRange(models);
                    await _dbContext.SaveChangesAsync();

                    return Ok(models);
                }
                else
                {
                    return BadRequest("Invalid JSON structure.");
                }
            }
        }
    }
}
