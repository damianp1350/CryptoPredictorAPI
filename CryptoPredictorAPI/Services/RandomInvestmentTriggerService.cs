using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services
{
    /// <summary>
    /// This service is for testing purposes only.
    /// </summary>
    public class RandomInvestmentTriggerService : IRandomInvestmentTriggerService
    {
        private readonly IBinanceService _binanceService;
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly ILogger<RandomInvestmentTriggerService> _logger;
        private readonly Random _random = new Random();

        public RandomInvestmentTriggerService(
            IBinanceService binanceService,
            IBinanceTestnetService binanceTestnetService,
            ILogger<RandomInvestmentTriggerService> logger)
        {
            _binanceService = binanceService;
            _binanceTestnetService = binanceTestnetService;
            _logger = logger;
        }

        public Task TriggerInvestment()
        {
            return Task.Run(async () =>
            {
                var randomNumber = _random.NextDouble() * 100;

                _logger.LogInformation($"Random number generated: {randomNumber}");

                if (randomNumber >= 90)
                {
                    _logger.LogInformation("Random number is greater than or equal to 90, initiating investment.");
                    await InitiateInvestmentAsync();
                }
            });
        }

        private async Task InitiateInvestmentAsync()
        {
            var symbol = "BTCUSDT";
            var quantity = 0.01m;

            var price = await _binanceService.FetchPrice(symbol);
            if (price.HasValue)
            {
                var response = await _binanceTestnetService.MakeTestInvestment(symbol, quantity, price.Value);
                _logger.LogInformation($"Investment response: {response}");
            }
            else
            {
                _logger.LogError("Failed to fetch the current price.");
            }
        }
    }
}
