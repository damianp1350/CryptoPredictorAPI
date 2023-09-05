using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface ICandleColorDeterminerService
    {
        CandleColor DetermineCandleColor(decimal open, decimal close);
    }
}