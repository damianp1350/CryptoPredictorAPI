using Microsoft.EntityFrameworkCore;
using CryptoPredictorApi.Models;

public class BinanceDbContext : DbContext
{
    public BinanceDbContext(DbContextOptions<BinanceDbContext> options) : base(options) { }

    public DbSet<BinanceKlineModel> BinanceHistoricalData { get; set; }
}
