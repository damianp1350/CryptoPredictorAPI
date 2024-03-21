using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;
using Hangfire;

namespace CryptoPredictorAPI.Services
{
    public class TestnetInvestmentService : ITestnetInvestmentService
    {
        private readonly IBinanceService _binanceService;
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly IBinanceJsonDeserializer _binanceJsonDeserializer;
        private readonly ILogger<TestnetInvestmentService> _logger;
        private readonly BinanceDbContext _dbContext;

        public TestnetInvestmentService(
            IBinanceService binanceService,
            IBinanceTestnetService binanceTestnetService,
            IBinanceJsonDeserializer binanceJsonDeserializer,
            ILogger<TestnetInvestmentService> logger,
            BinanceDbContext dbContext)
        {
            _binanceService = binanceService;
            _binanceTestnetService = binanceTestnetService;
            _binanceJsonDeserializer = binanceJsonDeserializer;
            _logger = logger;
            _dbContext = dbContext;
        }

        public void ScheduleInvestment()
        {
            RecurringJob.AddOrUpdate("TestnetInvestment", () => TriggerInvestment(), "* * * * *");
        }

        public async Task<(double? PredictedPrice, BinanceResponse Response)> TriggerInvestment()
        {
            var latestPrediction = GetLatestPredictedPrice();
            if (latestPrediction == null || latestPrediction.PredictedAt.Date != DateTime.UtcNow.Date)
            {
                _logger.LogInformation("No valid prediction for today's date. No investment will be made.");
                return (null, null);
            }

            double? predictedPrice = latestPrediction.Price;
            decimal? currentMarketPriceDecimal = await _binanceService.FetchPrice("BTCUSDT");
            double? currentMarketPrice = (double?)currentMarketPriceDecimal;

            if (predictedPrice.HasValue && currentMarketPrice.HasValue && predictedPrice <= currentMarketPrice) // >= normalnie <= dla testow
            {
                _logger.LogInformation($"Predicted price {predictedPrice.Value} is higher than the current market price {currentMarketPrice.Value}, initiating investment.");
                var response = await InitiateInvestmentAsync();
                return (predictedPrice, response);
            }

            _logger.LogInformation($"Predicted price: {predictedPrice} is not higher than the current market price {currentMarketPrice}.");
            return (predictedPrice, null);
        }

        private PredictedPriceModel GetLatestPredictedPrice()
        {
            return _dbContext.PredictedPrices.OrderByDescending(p => p.PredictedAt).FirstOrDefault();
        }

        private async Task<BinanceResponse> InitiateInvestmentAsync()
        {
            var symbol = "BTCUSDT";
            var quantity = 0.01m;

            var price = await _binanceService.FetchPrice(symbol);
            if (price.HasValue)
            {
                var jsonResponse = await _binanceTestnetService.MakeTestInvestment(symbol, quantity, price.Value);
                var response = _binanceJsonDeserializer.Deserialize<BinanceResponse>(jsonResponse);
                _logger.LogInformation($"Investment response: {jsonResponse}");
                return response;
            }
            else
            {
                _logger.LogError("Failed to fetch the current price.");
                return null;
            }
        }
    }
}
