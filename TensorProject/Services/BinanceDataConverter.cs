using TensorProject.Models;
using TensorProject.Services.IServices;
using System.Text.Json;
using System.Globalization;

namespace TensorProject.Services
{
    public class BinanceDataConverter : IBinanceDataConverter
    {
        public List<BinanceKlineModel> ConvertKlineData(List<JsonElement> klineData)
        {
            return klineData.Select(kline =>
            {
                decimal ParseDecimal(string value)
                {
                    if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                    {
                        return result;
                    }
                    throw new FormatException($"Unable to convert '{value}' to decimal.");
                }

                return new BinanceKlineModel
                {
                    OpenTime = kline[0].GetInt64(),
                    Open = ParseDecimal(kline[1].GetString()),
                    High = ParseDecimal(kline[2].GetString()),
                    Low = ParseDecimal(kline[3].GetString()),
                    Close = ParseDecimal(kline[4].GetString()),
                    Volume = ParseDecimal(kline[5].GetString()),
                    CloseTime = kline[6].GetInt64(),
                    QuoteAssetVolume = ParseDecimal(kline[7].GetString()),
                    NumberOfTrades = kline[8].GetInt32(),
                    TakerBuyBaseAssetVolume = ParseDecimal(kline[9].GetString()),
                    TakerBuyQuoteAssetVolume = ParseDecimal(kline[10].GetString())
                };
            }).ToList();
        }
    }
}
