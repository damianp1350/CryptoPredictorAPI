namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITestnetAssetSellService
    {
        void ScheduleSell();
        Task TriggerAssetSell();
    }
}