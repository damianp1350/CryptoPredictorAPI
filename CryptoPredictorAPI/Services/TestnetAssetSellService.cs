using Hangfire;
using CryptoPredictorAPI.Services.IServices;
using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services
{
    /// <summary>
    /// This service is for testing purposes only.
    /// </summary>
    public class TestnetAssetSellService : ITestnetAssetSellService
    {
        private readonly IBinanceService _binanceService;
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly IBinanceJsonDeserializer _binanceJsonDeserializer;
        private readonly ILogger<TestnetAssetSellService> _logger;
        private readonly Random _random = new Random();

        public TestnetAssetSellService(
            IBinanceService binanceService,
            IBinanceTestnetService binanceTestnetService,
            IBinanceJsonDeserializer binanceJsonDeserializer,
            ILogger<TestnetAssetSellService> logger)
        {
            _binanceService = binanceService;
            _binanceTestnetService = binanceTestnetService;
            _binanceJsonDeserializer = binanceJsonDeserializer;
            _logger = logger;
        }
        public void ScheduleSell()
        {
            RecurringJob.AddOrUpdate("AssetSell", () => TriggerAssetSell(), "* * * * *");
        }
        public async Task TriggerAssetSell()
        {
            var randomNumber = _random.NextDouble() * 100;
            _logger.LogInformation($"Random number generated: {randomNumber}");

            if (randomNumber <= 30)
            {
                _logger.LogInformation("Random number is less than or equal to 30, initiating asset sell.");
                await InitiateAssetSellAsync();
            }
            else
            {
                _logger.LogInformation("Random number is greater than 30, no sell initiated.");
            }
        }

        private async Task<BinanceResponse> InitiateAssetSellAsync()
        {
            var symbol = "BTCUSDT";
            var quantity = 0.01m;

            var price = await _binanceService.FetchPrice(symbol);
            if (price.HasValue)
            {
                var jsonResponse = await _binanceTestnetService.MakeTestSell(symbol, quantity, price.Value);
                var response = _binanceJsonDeserializer.Deserialize<BinanceResponse>(jsonResponse);
                _logger.LogInformation($"Sell response: {jsonResponse}");
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
