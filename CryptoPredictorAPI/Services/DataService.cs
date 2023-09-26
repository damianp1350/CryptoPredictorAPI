﻿using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
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
        public double CalculatePercentageOfCloseDifference(decimal priceDifference, decimal lastClosePrice)
        {
            return (double)(priceDifference / lastClosePrice) * 100;
        }
    }
}