using System.Collections.Generic;
using System.IO;
using System.Text;
using TensorProject.Models;
using CsvHelper;
using System.Formats.Asn1;
using TensorProject.Services.IServices;

namespace TensorProject.Services;

public class CsvExportService : ICsvExportService
{
    private readonly BinanceDbContext _dbContext;

    public CsvExportService(BinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ExportDataToCsv(string filePath)
    {
        var data = _dbContext.BinanceHistoricalDatas.ToList();

        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(data);
        }
    }
}
