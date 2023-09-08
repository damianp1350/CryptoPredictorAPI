namespace TensorProject.Services.IServices
{
    public interface ICandleTrendPredictorService
    {
        double CalculatePriceRiseProbability(int negativeStreak, decimal pips);
        int CountConsecutiveNegativeCandles();
    }
}