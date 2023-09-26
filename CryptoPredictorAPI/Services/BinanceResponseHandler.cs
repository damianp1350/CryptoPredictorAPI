using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
{
    public class BinanceResponseHandler : IBinanceResponseHandler
    {
        private readonly ILogger<BinanceResponseHandler> _logger;

        public BinanceResponseHandler(ILogger<BinanceResponseHandler> logger)
        {
            _logger = logger;
        }

        public async Task<string> HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error: {response.StatusCode}, Content: {errorContent}");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
