using System.Text;
using CsvHelper;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services;

public class DatabaseCsvExportService : IDatabaseCsvExportService
{
    private readonly BinanceDbContext _dbContext;

    public DatabaseCsvExportService(BinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ExportDataToCsv(string filePath)
    {
        var data = _dbContext.BinanceHistoricalData.ToList();

        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(data);
        }
    }
}
