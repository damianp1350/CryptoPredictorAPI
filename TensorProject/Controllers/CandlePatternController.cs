using Microsoft.AspNetCore.Mvc;
using TensorProject.Services.IServices;

namespace TensorProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandlePatternController : ControllerBase
    {
        private readonly ICandlePatternAnalyzerService _candlePatternAnalyzerService;

        public CandlePatternController(ICandlePatternAnalyzerService candlePatternAnalyzerService)
        {
            _candlePatternAnalyzerService = candlePatternAnalyzerService;
        }

        [HttpGet]
        [Route("patterns/probabilities")]
        public IActionResult GetPatternProbabilities()
        {
            var patternProbabilities = _candlePatternAnalyzerService.CalculateNextCandleProbabilities();
            return Ok(patternProbabilities);
        }
    }
}
