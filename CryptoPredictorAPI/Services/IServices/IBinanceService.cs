using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface IBinanceService
    {
        Task<List<BinanceKlineModel>> FetchAllHistoricalData(string symbol, long? startTime = null, long? endTime = null);
        Task<List<List<object>>> FetchHistoricalData(string symbol, string interval, int limit = 500);
        Task<List<List<object>>> FetchHistoricalData24h(string symbol);
        Task<decimal?> FetchPrice(string symbol);
    }
}