namespace CryptoPredictorApi.Services.IServices
{
    public interface IHistoricalDataRetrievalService
    {
        Task FetchAllHistoricalDataAutomated(string symbol);
        void ScheduleHistoricalDataRetrieval();
    }
}