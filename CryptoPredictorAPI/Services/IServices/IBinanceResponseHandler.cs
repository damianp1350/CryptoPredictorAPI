namespace CryptoPredictorAPI.Services.IServices;

public interface IBinanceResponseHandler
{
    Task<string> HandleResponse(HttpResponseMessage response);
}