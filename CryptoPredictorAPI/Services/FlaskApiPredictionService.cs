using CryptoPredictorAPI.Services.IServices;
using Hangfire;

namespace CryptoPredictorAPI.Services;

public class FlaskApiPredictionService : IFlaskApiPredictionService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FlaskApiPredictionService> _logger;
    public FlaskApiPredictionService(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<FlaskApiPredictionService> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public void SchedulePrediction()
    {
        RecurringJob.AddOrUpdate("ApiCallPrediction", () => TriggerPrediction(), Cron.Minutely);
    }

    public void StopPrediction()
    {
        RecurringJob.RemoveIfExists("ApiCallPrediction");
    }

    public async Task TriggerPrediction()
    {
        var filePath = _configuration.GetValue<string>("CsvExportSettings:FilePath");

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("CSV file not found at path: {FilePath}. Prediction job will not proceed.", filePath);
            return;
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var flaskApiService = scope.ServiceProvider.GetRequiredService<IFlaskApiService>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var file = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                await flaskApiService.GetPredictionFromFlask(file);
            }
        }
    }
}
