using Microsoft.AspNetCore.Mvc;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HangfireJobsController : ControllerBase
{
    private readonly ITestnetInvestmentService _testnetInvestmentService;
    private readonly ITestnetAssetSellService _testnetAssetSellService;
    private readonly IFlaskApiPredictionService _flaskApiPredictionService;
    private readonly IBinanceAutoDataRetrievalService _binanceAutoDataRetrievalService;

    public HangfireJobsController(
        ITestnetInvestmentService testnetInvestmentService,
        ITestnetAssetSellService testnetAssetSellService,
        IFlaskApiPredictionService flaskApiPredictionService,
        IBinanceAutoDataRetrievalService binanceAutoDataRetrievalService)
    {
        _testnetInvestmentService = testnetInvestmentService;
        _testnetAssetSellService = testnetAssetSellService;
        _flaskApiPredictionService = flaskApiPredictionService;
        _binanceAutoDataRetrievalService = binanceAutoDataRetrievalService;
    }

    [HttpPost("startInvestmentProcess")]
    public IActionResult StartInvestmentProcess()
    {
        _testnetInvestmentService.ScheduleInvestment();
        return Ok("Investment process started.");
    }

    [HttpPost("stopInvestmentProcess")]
    public IActionResult StopInvestmentProcess()
    {
        _testnetInvestmentService.StopInvestment();
        return Ok("Investment process stopped.");
    }

    [HttpPost("startAssetSellProcess")]
    public IActionResult StartAssetSellProcess()
    {
        _testnetAssetSellService.ScheduleSell();
        return Ok("Asset sell process started.");
    }

    [HttpPost("stopAssetSellProcess")]
    public IActionResult StopAssetSellProcess()
    {
        _testnetAssetSellService.StopSell();
        return Ok("Asset sell process stopped.");
    }

    [HttpPost("startPredictionProcess")]
    public IActionResult StartPredictionProcess()
    {
        _flaskApiPredictionService.SchedulePrediction();
        return Ok("Prediction process started.");
    }

    [HttpPost("stopPredictionProcess")]
    public IActionResult StopPredictionProcess()
    {
        _flaskApiPredictionService.StopPrediction();
        return Ok("Prediction process stopped.");
    }

    [HttpPost("startAllDataRetrievalProcess")]
    public IActionResult StartAllDataRetrievalProcess()
    {
        _binanceAutoDataRetrievalService.ScheduleAllDataRetrieval();
        return Ok("All data retrieval process started.");
    }

    [HttpPost("stopAllDataRetrievalProcess")]
    public IActionResult StopAllDataRetrievalProcess()
    {
        _binanceAutoDataRetrievalService.StopAllDataRetrieval();
        return Ok("All data retrieval process stopped.");
    }
}
