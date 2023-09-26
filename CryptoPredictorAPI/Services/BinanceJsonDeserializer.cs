using System.Text.Json;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
{
    public class BinanceJsonDeserializer : IBinanceJsonDeserializer
    {
        public T Deserialize<T>(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<T>(data, options);
        }
    }
}
