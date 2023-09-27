using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvExportController : ControllerBase
{
    private readonly IDatabaseCsvExportService _databaseCsvExportService;
    private readonly CsvExportSettings _csvExportSettings;

    public CsvExportController(IDatabaseCsvExportService databaseCsvExportService, IOptions<CsvExportSettings> csvExportSettings)
    {
        _databaseCsvExportService = databaseCsvExportService;
        _csvExportSettings = csvExportSettings.Value;
    }

    [HttpGet("export")]
    public IActionResult ExportDataToCsv()
    {
        string filePath = _csvExportSettings.FilePath;

        _databaseCsvExportService.ExportDataToCsv(filePath);

        return Ok($"Data has been exported to the file: {filePath}");
    }
}
