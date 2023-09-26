using Microsoft.AspNetCore.Mvc;
using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

[Route("api/[controller]")]
[ApiController]
public class TensorFlowController : ControllerBase
{
    private readonly ITensorFlowModelService _tensorFlowModelService;

    public TensorFlowController(ITensorFlowModelService tensorFlowModelService)
    {
        _tensorFlowModelService = tensorFlowModelService;
    }

    [HttpPost("predict-with-model")]
    public IActionResult Predict([FromBody] BitcoinPriceInput input)
    {
        if (input == null || input.CloseTime == 0)
        {
            return BadRequest("Invalid input data.");
        }

        var predictedPriceOutput = _tensorFlowModelService.Predict(input);

        return Ok(predictedPriceOutput);
    }
}