using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;
using Hangfire;

namespace CryptoPredictorAPI.Services
{
    /// <summary>
    /// This service is for testing purposes only.
    /// </summary>
    public class TestnetInvestmentService : ITestnetInvestmentService
    {
        private readonly IBinanceService _binanceService;
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly IBinanceJsonDeserializer _binanceJsonDeserializer;
        private readonly ILogger<TestnetInvestmentService> _logger;
        private readonly IFlaskApiService _flaskApiService;
        private double? _predictedPrice;

        public TestnetInvestmentService(
            IBinanceService binanceService,
            IBinanceTestnetService binanceTestnetService,
            IBinanceJsonDeserializer binanceJsonDeserializer,
            ILogger<TestnetInvestmentService> logger,
            IFlaskApiService flaskApiService)
        {
            _binanceService = binanceService;
            _binanceTestnetService = binanceTestnetService;
            _binanceJsonDeserializer = binanceJsonDeserializer;
            _logger = logger;
            _flaskApiService = flaskApiService;
        }

        public void ScheduleInvestment()
        {
            RecurringJob.AddOrUpdate("TestnetInvestment", () => TriggerInvestment(), "* * * * *");
        }

        public async Task SetPredictedPriceAsync(IFormFile file)
        {
            _predictedPrice = await _flaskApiService.GetPredictionFromFlask(file); // TODO: Fetch price correctly
        }

        public async Task<(double? PredictedPrice, BinanceResponse Response)> TriggerInvestment()
        {
            if (_predictedPrice.HasValue && _predictedPrice >= 40000) // TODO: if(predictedPrice >= currentPrice) -> invest - same for sell but with <=
            {
                _logger.LogInformation($"Predicted price {_predictedPrice.Value} is favorable for investment.");
                var response = await InitiateInvestmentAsync();
                return (_predictedPrice, response);
            }

            _logger.LogInformation($"Predicted price: {_predictedPrice.Value} is not favorable for investment or not set.");
            return (_predictedPrice, null);
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
