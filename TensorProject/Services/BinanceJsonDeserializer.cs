using System.Text.Json;
using TensorProject.Services.IServices;

namespace TensorProject.Services
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
