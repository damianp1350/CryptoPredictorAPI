namespace TensorProject.Services.IServices
{
    public interface ICandlePatternAnalyzerService
    {
        Dictionary<string, string> CalculateNextCandleProbabilities();
    }
}
