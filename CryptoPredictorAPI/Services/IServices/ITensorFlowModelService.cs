using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ITensorFlowModelService
    {
        BitcoinPriceOutput Predict(BitcoinPriceInput input);
    }
}