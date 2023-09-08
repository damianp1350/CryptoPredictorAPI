using Microsoft.AspNetCore.Mvc;
using TensorProject.Services.IServices;

[Route("api/[controller]")]
[ApiController]
public class CandleAnalysisController : ControllerBase
{
    private readonly ICandleTrendPredictorService _candleTrendPredictorService;

    public CandleAnalysisController(ICandleTrendPredictorService candleTrendPredictorService)
    {
        _candleTrendPredictorService = candleTrendPredictorService;
    }

    [HttpGet("negative-sequence-analysis/{symbol}")]
    public IActionResult AnalyzeNegativeCandleSequences(string symbol)
    {
        var negativeStreak = _candleTrendPredictorService.CountConsecutiveNegativeCandles();
        return Ok(new { NegativeStreak = negativeStreak });
    }

    [HttpGet("probability-analysis/{symbol}")]
    public IActionResult CalculateProbabilityOfPriceRise(string symbol, decimal pips)
    {
        var negativeStreak = _candleTrendPredictorService.CountConsecutiveNegativeCandles();
        var probability = _candleTrendPredictorService.CalculatePriceRiseProbability(negativeStreak, pips);
        return Ok(new { Probability = probability });
    }
}
