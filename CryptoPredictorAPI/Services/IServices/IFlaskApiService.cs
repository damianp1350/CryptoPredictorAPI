public interface IFlaskApiService
{
    Task<double?> GetPredictionFromFlask(IFormFile file);
}