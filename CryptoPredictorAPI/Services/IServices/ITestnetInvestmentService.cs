using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices;

public interface ITestnetInvestmentService
{
    void ScheduleInvestment();
    void StopInvestment();
    Task<(double? PredictedPrice, BinanceResponse Response)> TriggerInvestment();
}