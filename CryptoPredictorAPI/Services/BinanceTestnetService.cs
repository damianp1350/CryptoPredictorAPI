using System.Security.Cryptography;
using System.Text;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
{
    public class BinanceTestnetService : IBinanceTestnetService
    {
        private readonly HttpClient _client;
        private readonly IBinanceHttpRequestMessageCreator _messageCreator;
        private readonly IBinanceResponseHandler _responseHandler;
        private readonly string _testnetApiSecret;

        public BinanceTestnetService(
            IHttpClientFactory httpClientFactory,
            IBinanceHttpRequestMessageCreator messageCreator,
            IBinanceResponseHandler responseHandler,
            IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient("BinanceTestnetClient");
            _messageCreator = messageCreator;
            _responseHandler = responseHandler;
            _testnetApiSecret = configuration["BinanceTestnetSettings:ApiSecret"];
        }

        public async Task<string> MakeTestInvestment(string symbol, decimal quantity, decimal price)
        {
            var request = _messageCreator.CreateTestInvestmentRequest(symbol, quantity, price, CreateSignature);
            var response = await _client.SendAsync(request);
            var responseData = await _responseHandler.HandleResponse(response);

            return responseData;
        }

        private string CreateSignature(string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_testnetApiSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
