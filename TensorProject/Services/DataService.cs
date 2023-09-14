using TensorProject.Models;
using TensorProject.Services.IServices;

namespace TensorProject.Services
{
    public class DataService : IDataService
    {
        private readonly BinanceDbContext _dbContext;

        public DataService(BinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BinanceKlineModel> GetHistoricalData()
        {
            return _dbContext.BinanceHistoricalData.OrderBy(data => data.OpenTime).ToList();
        }
    }
}