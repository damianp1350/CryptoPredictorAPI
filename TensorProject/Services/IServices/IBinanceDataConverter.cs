using System.Text.Json;
using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface IBinanceDataConverter
    {
        public List<BinanceKlineModel> ConvertKlineData(List<JsonElement> klineData);
    }
}