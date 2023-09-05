using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TensorProject.Services.IServices;

namespace TensorProject.Services;

public class BinanceHttpRequestMessageCreator : IBinanceHttpRequestMessageCreator
{
    private readonly string _apiKey;
    private readonly string _apiSecret;

    public BinanceHttpRequestMessageCreator(IConfiguration configuration)
    {
        _apiKey = configuration["BinanceSettings:ApiKey"];
        _apiSecret = configuration["BinanceSettings:ApiSecret"];
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
}
