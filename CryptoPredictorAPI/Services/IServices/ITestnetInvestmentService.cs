using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITestnetInvestmentService
    {
        void ScheduleInvestment();
        Task<(double? PredictedPrice, BinanceResponse Response)> TriggerInvestment();
    }
}