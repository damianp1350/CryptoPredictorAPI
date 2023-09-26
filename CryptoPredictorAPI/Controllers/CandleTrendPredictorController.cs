using Microsoft.AspNetCore.Mvc;
using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

[Route("api/[controller]")]
[ApiController]
public class CandleTrendPredictorController : ControllerBase
{
    private readonly ICandleTrendPredictorService _candleTrendPredictorService;
    private readonly IDataService _dataService;

    public CandleTrendPredictorController(ICandleTrendPredictorService candleTrendPredictorService, IDataService dataService)
    {
        _candleTrendPredictorService = candleTrendPredictorService;
        _dataService = dataService;
    }

    [HttpGet("composite-probability/{priceDifference}")]
    public IActionResult GetCompositeProbability(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateCompositeProbability(historicalData, priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { CompositeProbability = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-next-high/{priceDifference}")]
    public IActionResult GetProbabilityOfNextHigh(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateNextHighProbability(historicalData, priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { ProbabilityOfNextHigh = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-next-high-with-volume/{priceDifference}")]
    public IActionResult GetProbabilityWithVolume(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithVolume(historicalData, priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { ProbabilityWithVolume = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-next-high-with-sma/{priceDifference}")]
    public IActionResult GetProbabilityWithSMA(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithSMA(historicalData, period: 10, priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { ProbabilityWithSMA = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-next-high-with-rsi/{priceDifference}")]
    public IActionResult GetProbabilityWithRSI(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithRSI(historicalData, period: 14, priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { ProbabilityWithRSI = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-with-bollinger-bands/{priceDifference}")]
    public IActionResult GetProbabilityWithBollingerBands(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithBollingerBands(historicalData, priceDifference: priceDifference);
        return Ok(new { ProbabilityWithBollingerBands = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-with-macd/{priceDifference}")]
    public IActionResult GetProbabilityWithMACD(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithMACD(historicalData, priceDifference: priceDifference);
        return Ok(new { ProbabilityWithMACD = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-with-stochastic-oscillator/{priceDifference}")]
    public IActionResult GetProbabilityWithStochasticOscillator(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithStochasticOscillator(historicalData, priceDifference: priceDifference);
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        return Ok(new { ProbabilityWithStochasticOscillator = probability, PercentageOfCloseDifference });
    }

    [HttpGet("probability-with-parabolic-sar/{priceDifference}")]
    public IActionResult GetProbabilityWithParabolicSAR(decimal priceDifference)
    {
        List<BinanceKlineModel> historicalData = _dataService.GetHistoricalData();
        decimal lastClosePrice = historicalData.Last().Close;
        var PercentageOfCloseDifference = _dataService.CalculatePercentageOfCloseDifference(priceDifference, lastClosePrice);
        var probability = _candleTrendPredictorService.CalculateNextHighProbabilityWithParabolicSAR(historicalData, priceDifference: priceDifference);
        return Ok(new { ProbabilityWithParabolicSAR = probability, PercentageOfCloseDifference });
    }
}
