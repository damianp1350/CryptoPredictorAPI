using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITestnetInvestmentService
    {
        void ScheduleInvestment();
        Task SetPredictedPriceAsync(IFormFile file);
        Task<(double? PredictedPrice, BinanceResponse Response)> TriggerInvestment();
    }
}