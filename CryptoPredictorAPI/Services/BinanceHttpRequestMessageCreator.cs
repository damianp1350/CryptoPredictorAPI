﻿using System.Globalization;
using System.Text;
using CryptoPredictorAPI.Services.IServices;
using System.Net.Http.Headers;

namespace CryptoPredictorAPI.Services;

public class BinanceHttpRequestMessageCreator : IBinanceHttpRequestMessageCreator
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly string _testnetApiKey;
    private readonly string _testnetApiSecret;
    private readonly ILogger<BinanceHttpRequestMessageCreator> _logger;

    public BinanceHttpRequestMessageCreator(IConfiguration configuration, ILogger<BinanceHttpRequestMessageCreator> logger)
    {
        _apiKey = configuration["BinanceSettings:ApiKey"];
        _apiSecret = configuration["BinanceSettings:ApiSecret"];
        _testnetApiKey = configuration["BinanceTestnetSettings:ApiKey"];
        _testnetApiSecret = configuration["BinanceTestnetSettings:ApiSecret"];
        _logger = logger;
    }

    public HttpRequestMessage CreatePriceRequestMessage(string symbol)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.binance.com/api/v3/ticker/price?symbol={symbol}"),
        };

        return request;
    }

    public HttpRequestMessage CreateHistoricalDataRequestMessage(string symbol, string interval, int limit = 500)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval}&limit={limit}"),
        };

        return request;
    }
    public HttpRequestMessage CreateHistoricalKlinesRequestMessage(string symbol, string interval, int limit, long? startTime = null, long? endTime = null)
    {
        var requestUrl = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval}&limit={limit}";

        if (startTime.HasValue)
        {
            requestUrl += $"&startTime={startTime.Value}";
        }

        if (endTime.HasValue)
        {
            requestUrl += $"&endTime={endTime.Value}";
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(requestUrl),
        };

        return request;
    }

    public HttpRequestMessage CreateTestInvestmentRequest(string symbol, decimal quantity, decimal price, Func<string, string> createSignature)
    {
        var quantityString = quantity.ToString(CultureInfo.InvariantCulture);
        var priceString = price.ToString(CultureInfo.InvariantCulture);

        var message = $"symbol={symbol}&side=BUY&type=LIMIT&timeInForce=GTC&quantity={quantityString}&price={priceString}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        var signature = createSignature(message);
        message += $"&signature={signature}";

        _logger.LogInformation($"Request message: {message}");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://testnet.binance.vision/api/v3/order"),
            Headers =
        {
            { "X-MBX-APIKEY", _testnetApiKey }
        },
            Content = new StringContent(
                message,
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            )
        };

        return request;
    }

    public HttpRequestMessage CreateTestSellRequest(string symbol, decimal quantity, decimal price, Func<string, string> createSignature)
    {
        var quantityString = quantity.ToString(CultureInfo.InvariantCulture);
        var priceString = price.ToString(CultureInfo.InvariantCulture);

        var message = $"symbol={symbol}&side=SELL&type=LIMIT&timeInForce=GTC&quantity={quantityString}&price={priceString}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        var signature = createSignature(message);
        message += $"&signature={signature}";

        _logger.LogInformation($"Request message: {message}");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://testnet.binance.vision/api/v3/order"),
            Headers =
        {
            { "X-MBX-APIKEY", _testnetApiKey }
        },
            Content = new StringContent(
                message,
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            )
        };

        return request;
    }

    public HttpRequestMessage CreateFlaskPredictRequest(string filePath)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("http://127.0.0.1:5001/predict"),
        };

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

        content.Add(fileContent, "file", Path.GetFileName(filePath));

        request.Content = content;

        return request;
    }
}
