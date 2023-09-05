using Microsoft.AspNetCore.Mvc;
using TensorProject.Models;
using TensorProject.Services;

[Route("api/[controller]")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly BinanceDbContext _context;

    public PredictionController(BinanceDbContext dbContext)
    {
        _context = dbContext;
    }
}
