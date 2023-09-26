using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services;

public class CandleColorDeterminerService : ICandleColorDeterminerService
{
    public CandleColor DetermineCandleColor(decimal open, decimal close)
    {
        return close > open ? CandleColor.Green : CandleColor.Red;
    }
}
