using Microsoft.AspNetCore.Mvc;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinanceTestnetController : ControllerBase
    {
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly ITestnetInvestmentService _testnetInvestmentService;

        public BinanceTestnetController(
            IBinanceTestnetService binanceTestnetService,
            ITestnetInvestmentService testnetInvestmentService)
        {
            _binanceTestnetService = binanceTestnetService;
            _testnetInvestmentService = testnetInvestmentService;
        }

        [HttpPost("testInvestment")]
        public async Task<IActionResult> TestInvestment(string symbol, decimal quantity, decimal price)
        {
            var result = await _binanceTestnetService.MakeTestInvestment(symbol, quantity, price);
            return Ok(result);
        }

        [HttpPost("triggerInvestmentDecision")]
        public async Task<IActionResult> TriggerInvestmentDecision()
        {
            var (predictedPrice, response) = await _testnetInvestmentService.TriggerInvestment();
            return Ok(new
            {
                PredictedPrice = predictedPrice,
                InvestmentResponse = response
            });
        }
    }
}
