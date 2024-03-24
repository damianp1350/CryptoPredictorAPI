using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITestnetAssetSellService
    {
        void ScheduleSell();
        Task<(double? PredictedPrice, BinanceResponse Response)> TriggerAssetSell();
    }
}