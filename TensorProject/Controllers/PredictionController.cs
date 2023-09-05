using Microsoft.AspNetCore.Mvc;
using TensorProject.Models;
using TensorProject.Services;

[Route("api/[controller]")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly TensorFlowModelService _tensorFlowModelService;

    public PredictionController()
    {
        _tensorFlowModelService = new TensorFlowModelService();
    }

    [HttpPost("predict")]
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