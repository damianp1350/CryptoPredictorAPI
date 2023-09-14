using TensorProject.Models;

namespace TensorProject.Services.IServices
{
    public interface ICandleTrendPredictorService
    {
        double CalculateCompositeProbability(List<BinanceKlineModel> historicalData, decimal priceDifference = 0);
        double CalculateNextHighProbability(List<BinanceKlineModel> historicalData, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithBollingerBands(List<BinanceKlineModel> historicalData, int period = 20, double multiplier = 2, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithMACD(List<BinanceKlineModel> historicalData, int shortPeriod = 12, int longPeriod = 26, int signalPeriod = 9, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithParabolicSAR(List<BinanceKlineModel> historicalData, double startAF = 0.02, double incrementAF = 0.02, double maxAF = 0.2, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithRSI(List<BinanceKlineModel> historicalData, int period = 14, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithSMA(List<BinanceKlineModel> historicalData, int period = 10, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithStochasticOscillator(List<BinanceKlineModel> historicalData, int period = 14, int signalPeriod = 3, decimal priceDifference = 0);
        double CalculateNextHighProbabilityWithVolume(List<BinanceKlineModel> historicalData, decimal priceDifference = 0);
    }
}