using System.Text.Json;
using CryptoPredictorApi.Models;

namespace CryptoPredictorApi.Services.IServices
{
    public interface IBinanceDataConverter
    {
        public List<BinanceKlineModel> ConvertKlineData(List<JsonElement> klineData);
    }
}