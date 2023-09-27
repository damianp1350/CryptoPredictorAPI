namespace CryptoPredictorAPI.Services.IServices
{
    public interface IDatabaseCsvExportService
    {
        void ExportDataToCsv(string filePath);
    }
}