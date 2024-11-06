namespace CryptoPredictorAPI.Services.IServices;

public interface IFlaskApiPredictionService
{
    void SchedulePrediction();
    void StopPrediction();
    Task TriggerPrediction();
}