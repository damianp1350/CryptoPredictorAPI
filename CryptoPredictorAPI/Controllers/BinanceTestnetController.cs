using Microsoft.AspNetCore.Mvc;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinanceTestnetController : ControllerBase
    {
        private readonly IBinanceTestnetService _binanceTestnetService;
        private readonly ITestnetInvestmentService _randomInvestmentTriggerService;

        public BinanceTestnetController(
            IBinanceTestnetService binanceTestnetService,
            ITestnetInvestmentService randomInvestmentTriggerService)
        {
            _binanceTestnetService = binanceTestnetService;
            _randomInvestmentTriggerService = randomInvestmentTriggerService;
        }

        [HttpPost("testInvestment")]
        public async Task<IActionResult> TestInvestment(string symbol, decimal quantity, decimal price)
        {
            var result = await _binanceTestnetService.MakeTestInvestment(symbol, quantity, price);
            return Ok(result);
        }

        [HttpPost("triggerRandomInvestment")]
        public async Task<IActionResult> TriggerRandomInvestment()
        {
            var (randomNumber, response) = await _randomInvestmentTriggerService.TriggerInvestment();

            if (response != null)
            {
                return Ok(new
                {
                    RandomNumber = randomNumber,
                    InvestmentResponse = response
                });
            }

            return Ok(new
            {
                RandomNumber = randomNumber,
                Message = "Random number is less than 80, no investment was made."
            });
        }
    }
}
