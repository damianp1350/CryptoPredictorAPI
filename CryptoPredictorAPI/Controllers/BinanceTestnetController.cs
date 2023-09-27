using Microsoft.AspNetCore.Mvc;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinanceTestnetController : ControllerBase
    {
        private readonly IBinanceTestnetService _binanceTestnetService;

        public BinanceTestnetController(IBinanceTestnetService binanceTestnetService)
        {
            _binanceTestnetService = binanceTestnetService;
        }

        [HttpPost("testInvestment")]
        public async Task<IActionResult> TestInvestment(string symbol, decimal quantity, decimal price)
        {
            var result = await _binanceTestnetService.MakeTestInvestment(symbol, quantity, price);
            return Ok(result);
        }
    }
}
