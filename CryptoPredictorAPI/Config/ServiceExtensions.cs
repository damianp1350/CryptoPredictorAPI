using Hangfire;
using Microsoft.EntityFrameworkCore;
using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services;
using CryptoPredictorApi.Services.IServices;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBinanceHttpRequestMessageCreator, BinanceHttpRequestMessageCreator>();
        services.AddSingleton<IBinanceResponseHandler, BinanceResponseHandler>();
        services.AddSingleton<IBinanceJsonDeserializer, BinanceJsonDeserializer>();
        services.AddSingleton<IBinanceDataConverter, BinanceDataConverter>();

        services.AddScoped<IDataLoaderService, DataLoaderService>();
        services.AddScoped<ITensorFlowModelService, TensorFlowModelService>();
        services.AddScoped<ICandlePatternAnalyzerService, CandlePatternAnalyzerService>();
        services.AddScoped<IDataService, DataService>();
        services.AddScoped<ICandleTrendPredictorService, CandleTrendPredictorService>();
        services.AddScoped<IHistoricalDataRetrievalService, HistoricalDataRetrievalService>();
        services.AddScoped<IBinanceService, BinanceService>();
        services.AddScoped<IBinanceTestnetService, BinanceTestnetService>();

        services.AddTransient<ICsvExportService, CsvExportService>();

        services.AddHttpClient("BinanceTestnetClient");
        services.AddHttpClient("BinanceClient");

        services.AddDbContext<BinanceDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<CsvExportSettings>(configuration.GetSection("CsvExportSettings"));

        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
    }
}
