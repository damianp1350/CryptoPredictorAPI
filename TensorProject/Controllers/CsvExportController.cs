using Microsoft.AspNetCore.Mvc;
using TensorProject.Services.IServices;


namespace TensorProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvExportController : ControllerBase
{
    private readonly ICsvExportService _csvExportService;

    public CsvExportController(ICsvExportService csvExportService)
    {
        _csvExportService = csvExportService;
    }

    [HttpGet("export")]
    public IActionResult ExportDataToCsv()
    {
        string filePath = "PLACEHOLDER";

        _csvExportService.ExportDataToCsv(filePath);

        return Ok($"Dane zostały wyeksportowane do pliku: {filePath}");
    }
}
