using System.Text.Json.Serialization;

namespace CryptoPredictorAPI.Models;

public class TradeResponseFillModel
{
    [JsonPropertyName("price")]
    public string Price { get; set; }

    [JsonPropertyName("qty")]
    public string Quantity { get; set; }

    [JsonPropertyName("commission")]
    public string Commission { get; set; }

    [JsonPropertyName("commissionAsset")]
    public string CommissionAsset { get; set; }

    [JsonPropertyName("tradeId")]
    public long TradeId { get; set; }
}
