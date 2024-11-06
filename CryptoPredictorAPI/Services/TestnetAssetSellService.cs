using Hangfire;
using CryptoPredictorAPI.Services.IServices;
using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services;

public class TestnetAssetSellService : ITestnetAssetSellService
{
    private readonly IBinanceService _binanceService;
    private readonly IBinanceTestnetService _binanceTestnetService;
    private readonly IBinanceJsonDeserializer _binanceJsonDeserializer;
    private readonly ILogger<TestnetAssetSellService> _logger;
    private readonly BinanceDbContext _dbContext;

    public TestnetAssetSellService(
        IBinanceService binanceService,
        IBinanceTestnetService binanceTestnetService,
        IBinanceJsonDeserializer binanceJsonDeserializer,
        ILogger<TestnetAssetSellService> logger,
        BinanceDbContext dbContext)
    {
        _binanceService = binanceService;
        _binanceTestnetService = binanceTestnetService;
        _binanceJsonDeserializer = binanceJsonDeserializer;
        _logger = logger;
        _dbContext = dbContext;
    }

    public void ScheduleSell()
    {
        RecurringJob.AddOrUpdate("AssetSell", () => TriggerAssetSell(), "* * * * *");
    }

    public void StopSell()
    {
        RecurringJob.RemoveIfExists("AssetSell");
    }

    public async Task<(double? PredictedPrice, BinanceResponse Response)> TriggerAssetSell()
    {
        var latestPrediction = GetLatestPredictedPrice();
        if (latestPrediction == null || latestPrediction.PredictedAt.Date != DateTime.UtcNow.Date)
        {
            _logger.LogInformation("No valid prediction for today's date. No assets will be sold.");
            return (null, null);
        }

        double? predictedPrice = latestPrediction.Price;
        decimal? currentMarketPriceDecimal = await _binanceService.FetchPrice("BTCUSDT");
        double? currentMarketPrice = (double?)currentMarketPriceDecimal;

        if (predictedPrice.HasValue && currentMarketPrice.HasValue && predictedPrice <= currentMarketPrice)
        {
            _logger.LogInformation($"Predicted price {predictedPrice.Value} is lower than the current market price {currentMarketPrice.Value}, initiating asset sell.");
            var response = await InitiateAssetSellAsync();
            return (predictedPrice, response);
        }

        _logger.LogInformation($"Predicted price: {predictedPrice} is not higher than the current market price {currentMarketPrice}.");
        return (predictedPrice, null);
    }

    private PredictedPriceModel GetLatestPredictedPrice()
    {
        return _dbContext.PredictedPrices.OrderByDescending(p => p.PredictedAt).FirstOrDefault();
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
