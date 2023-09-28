using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITestnetInvestmentService
    {
        void ScheduleInvestment();
        Task<(double randomNumber, BinanceResponse response)> TriggerInvestment();
    }
}