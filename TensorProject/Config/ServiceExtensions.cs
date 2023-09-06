using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TensorProject.Services;
using TensorProject.Services.IServices;

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
        services.AddTransient<ICsvExportService, CsvExportService>();

        services.AddHttpClient("BinanceClient");
        services.AddScoped<IBinanceService, BinanceService>();

        services.AddDbContext<BinanceDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
