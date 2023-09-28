using System.Text.Json.Serialization;

namespace CryptoPredictorAPI.Models
{
    public class BinanceResponse
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }

        [JsonPropertyName("orderListId")]
        public long OrderListId { get; set; }

        [JsonPropertyName("clientOrderId")]
        public string ClientOrderId { get; set; }

        [JsonPropertyName("transactTime")]
        public long TransactTime { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("origQty")]
        public string OriginalQuantity { get; set; }

        [JsonPropertyName("executedQty")]
        public string ExecutedQuantity { get; set; }

        [JsonPropertyName("cummulativeQuoteQty")]
        public string CumulativeQuoteQuantity { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timeInForce")]
        public string TimeInForce { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("side")]
        public string Side { get; set; }

        [JsonPropertyName("workingTime")]
        public long WorkingTime { get; set; }

        [JsonPropertyName("fills")]
        public List<TradeResponseFillModel> Fills { get; set; }

        [JsonPropertyName("selfTradePreventionMode")]
        public string SelfTradePreventionMode { get; set; }
    }
}

