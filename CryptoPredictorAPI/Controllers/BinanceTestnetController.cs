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
            var (predictedPrice, response) = await _randomInvestmentTriggerService.TriggerInvestment();

            if (response != null)
            {
                return Ok(new
                {
                    RandomNumber = predictedPrice,
                    InvestmentResponse = response
                });
            }

            return Ok(new
            {
                RandomNumber = predictedPrice,
                Message = $"Predicted price: {predictedPrice} is less than: 40000 , no investment was made." // price is a placeholder
            });
        }
    }
}
