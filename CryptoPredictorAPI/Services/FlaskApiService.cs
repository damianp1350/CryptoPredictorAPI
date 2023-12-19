using CryptoPredictorAPI.Services.IServices;
using CryptoPredictorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoPredictorAPI.Services
{
    public class FlaskApiService : IFlaskApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IBinanceHttpRequestMessageCreator _messageCreator;
        private readonly IBinanceJsonDeserializer _jsonDeserializer;
        private readonly IBinanceResponseHandler _responseHandler;
        private readonly ILogger<FlaskApiService> _logger;

        public FlaskApiService(
            IHttpClientFactory httpClientFactory,
            IBinanceHttpRequestMessageCreator messageCreator,
            IBinanceJsonDeserializer jsonDeserializer,
            IBinanceResponseHandler responseHandler,
            ILogger<FlaskApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("FlaskApiService");
            _messageCreator = messageCreator;
            _jsonDeserializer = jsonDeserializer;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<double?> GetPredictionFromFlask(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("File is not provided or empty.");
                return null;
            }

            var filePath = Path.GetTempFileName();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var request = _messageCreator.CreateFlaskPredictRequest(filePath);
                var response = await _httpClient.SendAsync(request);
                var responseString = await _responseHandler.HandleResponse(response);

                if (string.IsNullOrEmpty(responseString))
                {
                    return null;
                }

                var predictionResponse = _jsonDeserializer.Deserialize<PredictionResponse>(responseString);
                return predictionResponse?.PredictedClosePrice;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the file: {ex.Message}");
                return null;
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
