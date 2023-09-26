using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvExportController : ControllerBase
{
    private readonly ICsvExportService _csvExportService;
    private readonly CsvExportSettings _csvExportSettings;

    public CsvExportController(ICsvExportService csvExportService, IOptions<CsvExportSettings> csvExportSettings)
    {
        _csvExportService = csvExportService;
        _csvExportSettings = csvExportSettings.Value;
    }

    [HttpGet("export")]
    public IActionResult ExportDataToCsv()
    {
        string filePath = _csvExportSettings.FilePath;

        _csvExportService.ExportDataToCsv(filePath);

        return Ok($"Data has been exported to the file: {filePath}");
    }
}
