namespace CryptoPredictorAPI.Services.IServices
{
    public interface IFlaskApiPredictionService
    {
        void SchedulePrediction();
        Task TriggerPrediction();
    }
}