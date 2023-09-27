namespace CryptoPredictorAPI.Services.IServices
{
    public interface IBinanceAutoDataRetrievalService
    {
        Task FetchAllHistoricalDataAutomated(string symbol);
        void ScheduleHistoricalDataRetrieval();
    }
}