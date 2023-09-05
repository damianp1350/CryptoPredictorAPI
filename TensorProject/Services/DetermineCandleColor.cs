using TensorProject.Models;
using TensorProject.Services.IServices;

namespace TensorProject.Services;

public class CandleColorDeterminerService : ICandleColorDeterminerService
{
    public CandleColor DetermineCandleColor(decimal open, decimal close)
    {
        return close > open ? CandleColor.Green : CandleColor.Red;
    }
}
