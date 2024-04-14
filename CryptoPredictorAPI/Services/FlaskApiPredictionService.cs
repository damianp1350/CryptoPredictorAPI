using CryptoPredictorAPI.Services.IServices;
using Hangfire;

namespace CryptoPredictorAPI.Services
{
    public class FlaskApiPredictionService : IFlaskApiPredictionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public FlaskApiPredictionService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public void SchedulePrediction()
        {
            RecurringJob.AddOrUpdate("ApiCallPrediction", () => TriggerPrediction(), Cron.Minutely);
        }

        public async Task TriggerPrediction()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var flaskApiService = scope.ServiceProvider.GetRequiredService<IFlaskApiService>();
                var filePath = _configuration.GetValue<string>("CsvExportSettings:FilePath");

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
}
