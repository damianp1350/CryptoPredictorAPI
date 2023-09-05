using System.Linq;
using Microsoft.EntityFrameworkCore;
using TensorProject.Models;
using TensorProject.Services.IServices;

namespace TensorProject.Services;

public class DataLoaderService : IDataLoaderService
{
    private readonly BinanceDbContext _dbContext;

    public DataLoaderService(BinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<BitcoinPriceData> LoadTrainingData()
    {
        return _dbContext.BinanceHistoricalDatas.Select(d => new BitcoinPriceData
        {
            Open = (float)d.Open,
            High = (float)d.High,
            Low = (float)d.Low,
            Volume = (float)d.Volume,
            Close = (float)d.Close
        }).ToList();
    }
}
