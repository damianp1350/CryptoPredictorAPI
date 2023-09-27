using Hangfire;
using Microsoft.EntityFrameworkCore;
using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services;
using CryptoPredictorAPI.Services.IServices;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBinanceHttpRequestMessageCreator, BinanceHttpRequestMessageCreator>();
        services.AddSingleton<IBinanceResponseHandler, BinanceResponseHandler>();
        services.AddSingleton<IBinanceJsonDeserializer, BinanceJsonDeserializer>();
        services.AddSingleton<IBinanceDataConverter, BinanceDataConverter>();

        services.AddScoped<IRandomInvestmentTriggerService, RandomInvestmentTriggerService>();
        services.AddScoped<ITensorFlowModelService, TensorFlowModelService>();
        services.AddScoped<ICandleTrendPredictorDataService, CandleTrendPredictorDataService>();
        services.AddScoped<ICandleTrendPredictorService, CandleTrendPredictorService>();
        services.AddScoped<IBinanceAutoDataRetrievalService, BinanceAutoDataRetrievalService>();
        services.AddScoped<IBinanceService, BinanceService>();
        services.AddScoped<IBinanceTestnetService, BinanceTestnetService>();

        services.AddTransient<IDatabaseCsvExportService, DatabaseCsvExportService>();

        services.AddHttpClient("BinanceTestnetClient");
        services.AddHttpClient("BinanceClient");

        services.AddDbContext<BinanceDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<CsvExportSettings>(configuration.GetSection("CsvExportSettings"));

        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
    }
}
