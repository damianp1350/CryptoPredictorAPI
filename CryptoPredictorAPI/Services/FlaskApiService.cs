using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services;

public class FlaskApiService : IFlaskApiService
{
    private readonly HttpClient _httpClient;
    private readonly IBinanceHttpRequestMessageCreator _messageCreator;
    private readonly IBinanceJsonDeserializer _jsonDeserializer;
    private readonly IBinanceResponseHandler _responseHandler;
    private readonly ILogger<FlaskApiService> _logger;
    private readonly BinanceDbContext _dbContext;

    public FlaskApiService(
        IHttpClientFactory httpClientFactory,
        IBinanceHttpRequestMessageCreator messageCreator,
        IBinanceJsonDeserializer jsonDeserializer,
        IBinanceResponseHandler responseHandler,
        ILogger<FlaskApiService> logger,
        BinanceDbContext dbContext)
    {
        _httpClient = httpClientFactory.CreateClient("FlaskApiService");
        _messageCreator = messageCreator;
        _jsonDeserializer = jsonDeserializer;
        _responseHandler = responseHandler;
        _logger = logger;
        _dbContext = dbContext;
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
            if (predictionResponse != null)
            {
                SavePredictedPrice(predictionResponse.PredictedClosePrice);
                return predictionResponse.PredictedClosePrice;
            }
            return null;
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

    private void SavePredictedPrice(double price)
    {
        var predictedPrice = new PredictedPriceModel
        {
            Price = price,
            PredictedAt = DateTime.UtcNow
        };
        _dbContext.PredictedPrices.Add(predictedPrice);
        _dbContext.SaveChanges();
    }
}
