namespace CryptoPredictorAPI.Services.IServices
{
    public interface IBinanceTestnetService
    {
        Task<string> MakeTestInvestment(string symbol, decimal quantity, decimal price);
    }
}