using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices;

public interface ITestnetAssetSellService
{
    void ScheduleSell();
    void StopSell();
    Task<(double? PredictedPrice, BinanceResponse Response)> TriggerAssetSell();
}