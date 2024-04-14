using Microsoft.AspNetCore.Mvc;
using CryptoPredictorAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FlaskApiController : ControllerBase
{
    private readonly IFlaskApiService _flaskApiService;

    public FlaskApiController(IFlaskApiService flaskApiService)
    {
        _flaskApiService = flaskApiService;
    }

    [HttpPost("predict")]
    public async Task<IActionResult> PredictPrice([FromForm] IFormFile file)
    {
        var prediction = await _flaskApiService.GetPredictionFromFlask(file);
        return Ok(prediction);
    }
}
