using Microsoft.EntityFrameworkCore;
using TensorProject.Models;

public class BinanceDbContext : DbContext
{
    public BinanceDbContext(DbContextOptions<BinanceDbContext> options) : base(options) { }

    public DbSet<BinanceKlineModel> BinanceHistoricalDatas { get; set; }
}
