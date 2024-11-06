namespace CryptoPredictorAPI.Services.IServices;

public interface IBinanceJsonDeserializer
{
    T Deserialize<T>(string data);
}