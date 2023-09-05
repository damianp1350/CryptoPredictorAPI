using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface IDataLoaderService
    {
        IEnumerable<BitcoinPriceData> LoadTrainingData();
    }
}