namespace CryptoPredictorAPI.Services.IServices
{
    public interface IBinanceHttpRequestMessageCreator
    {
        HttpRequestMessage CreateHistoricalDataRequestMessage(string symbol, string interval, int limit = 500);
        HttpRequestMessage CreateHistoricalKlinesRequestMessage(string symbol, string interval, int limit, long? startTime = null, long? endTime = null);
        HttpRequestMessage CreatePriceRequestMessage(string symbol);
        HttpRequestMessage CreateTestInvestmentRequest(string symbol, decimal quantity, decimal price, Func<string, string> createSignature);
    }
}