namespace CryptoPredictorAPI.Services.IServices;

public interface IBinanceAutoDataRetrievalService
{
    Task FetchAllDataAutomated(string symbol);
    void ScheduleAllDataRetrieval();
    void StopAllDataRetrieval();
}