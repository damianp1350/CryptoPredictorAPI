using Hangfire;
using CryptoPredictorAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var testnetInvestmentService = services.GetRequiredService<ITestnetInvestmentService>();
    testnetInvestmentService.ScheduleInvestment();

    var testnetAssetSellService = services.GetRequiredService<ITestnetAssetSellService>();
    testnetAssetSellService.ScheduleSell();

    var binanceAutoDataRetrievalService = services.GetRequiredService<IBinanceAutoDataRetrievalService>();
    binanceAutoDataRetrievalService.ScheduleHistoricalDataRetrieval();
}

app.Run();