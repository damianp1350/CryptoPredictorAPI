using Microsoft.AspNetCore.Mvc;
using TensorProject.Services.IServices;

namespace TensorProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceService _binanceService;

        public BinanceController(IBinanceService binanceService)
        {
            _binanceService = binanceService;
        }

        [HttpGet("price/{symbol}")]
        public async Task<IActionResult> GetPrice(string symbol)
        {
            var price = await _binanceService.FetchPrice(symbol);

            if (price.HasValue)
                return Ok(price.Value);
            else
                return NotFound();
        }

        [HttpGet("historical/{symbol}/{interval}")]
        public async Task<IActionResult> GetHistoricalData(string symbol, string interval, int limit = 500)
        {
            var historicalData = await _binanceService.FetchHistoricalData(symbol, interval, limit);

            if (historicalData != null && historicalData.Any())
                return Ok(historicalData);
            else
                return NotFound();
        }

        [HttpGet("historical/last-24-hours/{symbol}")]
        public async Task<IActionResult> GetHistoricalData24h(string symbol)
        {
            var historicalData = await _binanceService.FetchHistoricalData24h(symbol);
            if (historicalData != null && historicalData.Any())
                return Ok(historicalData);
            else
                return NotFound();
        }

        [HttpGet("historical/all/{symbol}")]
        public async Task<IActionResult> GetAllHistoricalData(string symbol, long? startTime = null, long? endTime = null)
        {
            try
            {
                var models = await _binanceService.FetchAllHistoricalData(symbol, startTime, endTime);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
