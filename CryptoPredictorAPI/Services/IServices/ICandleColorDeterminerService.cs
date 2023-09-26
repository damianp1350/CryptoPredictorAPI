using CryptoPredictorApi.Models;

namespace CryptoPredictorApi.Services.IServices
{
    public interface ICandleColorDeterminerService
    {
        CandleColor DetermineCandleColor(decimal open, decimal close);
    }
}