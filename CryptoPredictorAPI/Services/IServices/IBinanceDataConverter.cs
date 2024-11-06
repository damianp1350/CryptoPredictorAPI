using System.Text.Json;
using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices;

public interface IBinanceDataConverter
{
    public List<BinanceKlineModel> ConvertKlineData(List<JsonElement> klineData);
}