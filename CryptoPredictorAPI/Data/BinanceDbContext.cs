using Microsoft.EntityFrameworkCore;
using CryptoPredictorAPI.Models;

public class BinanceDbContext : DbContext
{
    public BinanceDbContext(DbContextOptions<BinanceDbContext> options) : base(options) { }

    public DbSet<BinanceKlineModel> BinanceHistoricalData { get; set; }
    public DbSet<PredictedPriceModel> PredictedPrices { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinanceKlineModel>(entity =>
        {
            entity.Property(e => e.Open).HasPrecision(18, 2);
            entity.Property(e => e.High).HasPrecision(18, 2);
            entity.Property(e => e.Low).HasPrecision(18, 2);
            entity.Property(e => e.Close).HasPrecision(18, 2);
            entity.Property(e => e.Volume).HasPrecision(18, 2);
            entity.Property(e => e.QuoteAssetVolume).HasPrecision(18, 2);
            entity.Property(e => e.TakerBuyBaseAssetVolume).HasPrecision(18, 2);
            entity.Property(e => e.TakerBuyQuoteAssetVolume).HasPrecision(18, 2);
        });

        base.OnModelCreating(modelBuilder);
    }
}
