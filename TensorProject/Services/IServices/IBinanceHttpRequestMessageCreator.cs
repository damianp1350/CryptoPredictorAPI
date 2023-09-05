namespace TensorProject.Services.IServices
{
    public interface IBinanceHttpRequestMessageCreator
    {
        HttpRequestMessage CreatePriceRequestMessage(string symbol);
        HttpRequestMessage CreateHistoricalDataRequestMessage(string symbol, string interval, int limit = 500);
        public HttpRequestMessage CreateHistoricalKlinesRequestMessage(string symbol, string interval, int limit, long? startTime = null, long? endTime = null);

    }
}