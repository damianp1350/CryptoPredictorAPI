using System.Text.Json.Serialization;

namespace CryptoPredictorAPI.Models;

public class PredictionResponse
{
    [JsonPropertyName("predicted_close_price")]
    public double PredictedClosePrice { get; set; }
}