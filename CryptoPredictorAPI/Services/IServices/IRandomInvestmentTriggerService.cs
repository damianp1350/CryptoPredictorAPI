using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface IRandomInvestmentTriggerService
    {
        void ScheduleInvestment();
        Task<(double randomNumber, BinanceResponse response)> TriggerInvestment();
    }
}