using CryptoPredictorApi.Models;

namespace CryptoPredictorApi.Services.IServices
{
    public interface ITensorFlowModelService
    {
        BitcoinPriceOutput Predict(BitcoinPriceInput input);
    }
}