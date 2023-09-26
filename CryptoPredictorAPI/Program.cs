using Hangfire;
using CryptoPredictorApi.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

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

    var binanceService = services.GetRequiredService<IBinanceService>();
    RecurringJob.AddOrUpdate("fetch-latest-data-BTCUSDT", () => binanceService.FetchLatestHistoricalData("BTCUSDT"), "*/30 * * * *");

    var historicalDataRetrievalService = services.GetRequiredService<IHistoricalDataRetrievalService>();
    historicalDataRetrievalService.ScheduleHistoricalDataRetrieval();
}

app.Run();