using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface IDataService
    {
        List<BinanceKlineModel> GetHistoricalData();
    }
}