using System.Text.Json;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services;

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
