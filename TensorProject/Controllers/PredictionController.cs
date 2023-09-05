using Microsoft.AspNetCore.Mvc;

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
