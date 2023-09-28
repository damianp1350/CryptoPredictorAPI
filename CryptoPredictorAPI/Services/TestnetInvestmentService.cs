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
        private readonly Random _random = new Random();

        public TestnetInvestmentService(
            IBinanceService binanceService,
            IBinanceTestnetService binanceTestnetService,
            IBinanceJsonDeserializer binanceJsonDeserializer,
            ILogger<TestnetInvestmentService> logger)
        {
            _binanceService = binanceService;
            _binanceTestnetService = binanceTestnetService;
            _binanceJsonDeserializer = binanceJsonDeserializer;
            _logger = logger;
        }
        public void ScheduleInvestment()
        {
            RecurringJob.AddOrUpdate("TestnetInvestment", () => TriggerInvestment(), "* * * * *");
        }
        public async Task<(double randomNumber, BinanceResponse response)> TriggerInvestment()
        {
            var randomNumber = _random.NextDouble() * 100;
            _logger.LogInformation($"Random number generated: {randomNumber}");

            if (randomNumber >= 80)
            {
                _logger.LogInformation("Random number is greater than or equal to 80, initiating investment.");
                var response = await InitiateInvestmentAsync();
                return (randomNumber, response);
            }

            _logger.LogInformation("Random number is less than 80, no investment was made.");
            return (randomNumber, null);
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
