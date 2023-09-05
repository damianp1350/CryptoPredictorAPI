using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface ITensorFlowModelService
    {
        BitcoinPriceOutput Predict(BitcoinPriceInput input);
    }
}