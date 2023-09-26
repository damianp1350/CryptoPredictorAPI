using CryptoPredictorApi.Models;

namespace CryptoPredictorApi.Services.IServices
{
    public interface IDataLoaderService
    {
        IEnumerable<BitcoinPriceData> LoadTrainingData();
    }
}