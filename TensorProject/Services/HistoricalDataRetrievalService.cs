using TensorProject.Services.IServices;
using Hangfire;

namespace TensorProject.Services
{
    public class HistoricalDataRetrievalService : IHistoricalDataRetrievalService
    {
        private readonly BinanceDbContext _dbContext;
        private readonly IBinanceService _binanceService;
        private const int DaysToFetchPerRequest = 900;

        public HistoricalDataRetrievalService(BinanceDbContext dbContext, IBinanceService binanceService)
        {
            _dbContext = dbContext;
            _binanceService = binanceService;
        }

        public void ScheduleHistoricalDataRetrieval()
        {
            RecurringJob.AddOrUpdate("FetchHistoricalData", () => FetchAllHistoricalDataAutomated("BTCUSDT"), "*/2 * * * *");
        }

        [DisableConcurrentExecution(180)]
        public async Task FetchAllHistoricalDataAutomated(string symbol)
        {
            long? lastFetchedOpenTime = _dbContext.BinanceHistoricalData.Max(m => (long?)m.OpenTime);

            long _currentStartTime = lastFetchedOpenTime.HasValue
                ? lastFetchedOpenTime.Value + (24 * 60 * 60 * 1000L)
                : (long)(new DateTime(2017, 1, 1).Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            if (_currentStartTime >= GetCurrentUnixTime())
            {
                RecurringJob.RemoveIfExists("FetchHistoricalData");
                return;
            }

            long _currentEndTime = _currentStartTime + DaysToFetchPerRequest * 24 * 60 * 60 * 1000L;

            var newRecords = await _binanceService.FetchAllHistoricalData(symbol, _currentStartTime, _currentEndTime);

            var existingOpenTimes = _dbContext.BinanceHistoricalData.Select(m => m.OpenTime).ToHashSet();
            var recordsToAdd = newRecords.Where(m => !existingOpenTimes.Contains(m.OpenTime)).ToList();

            if (recordsToAdd.Count > 0)
            {
                _dbContext.BinanceHistoricalData.AddRange(recordsToAdd);
                await _dbContext.SaveChangesAsync();
            }
        }

        private long GetCurrentUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }
    }
}
