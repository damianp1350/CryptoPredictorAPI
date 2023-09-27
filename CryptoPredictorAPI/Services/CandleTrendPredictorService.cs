using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services
{
    public class CandleTrendPredictorService : ICandleTrendPredictorService
    {
        public CandleTrendPredictorService()
        {
        }

        public double CalculateNextHighProbability(List<BinanceKlineModel> historicalData, decimal priceDifference = 0)
        {
            int countOfHighsGreaterThanCloseByDifference = 0;

            for (int i = 0; i < historicalData.Count - 1; i++)
            {
                if (historicalData[i + 1].High > historicalData[i].Close + priceDifference)
                {
                    countOfHighsGreaterThanCloseByDifference++;
                }
            }

            return (double)countOfHighsGreaterThanCloseByDifference / (historicalData.Count - 1);
        }

        public double CalculateNextHighProbabilityWithVolume(List<BinanceKlineModel> historicalData, decimal priceDifference = 0)
        {
            int countOfHighsGreaterThanCloseByDifference = 0;
            double averageVolume = (double)historicalData.Average(data => (double)data.Volume);

            for (int i = 0; i < historicalData.Count - 1; i++)
            {
                if (historicalData[i + 1].High > historicalData[i].Close + priceDifference)
                {
                    countOfHighsGreaterThanCloseByDifference += historicalData[i + 1].Volume > (decimal)averageVolume ? (int)1.5 : 1;
                }
            }

            return (double)countOfHighsGreaterThanCloseByDifference / (historicalData.Count - 1);
        }

        public double CalculateNextHighProbabilityWithSMA(List<BinanceKlineModel> historicalData, int period = 10, decimal priceDifference = 0)
        {
            int countOfHighsGreaterThanSMAByDifference = 0;

            for (int i = period; i < historicalData.Count - 1; i++)
            {
                var sma = historicalData.Skip(i - period).Take(period).Average(data => data.Close);

                if (historicalData[i + 1].High > sma + priceDifference)
                {
                    countOfHighsGreaterThanSMAByDifference++;
                }
            }

            return (double)countOfHighsGreaterThanSMAByDifference / (historicalData.Count - period);
        }

        public double CalculateNextHighProbabilityWithRSI(List<BinanceKlineModel> historicalData, int period = 14, decimal priceDifference = 0)
        {
            int countOfHighsAfterOversoldByDifference = 0;

            List<decimal> gains = new List<decimal>();
            List<decimal> losses = new List<decimal>();

            for (int i = 1; i < historicalData.Count; i++)
            {
                var difference = historicalData[i].Close - historicalData[i - 1].Close;
                gains.Add(Math.Max(difference, 0));
                losses.Add(Math.Max(-difference, 0));
            }

            List<decimal> averageGains = new List<decimal> { gains.Take(period).Average() };
            List<decimal> averageLosses = new List<decimal> { losses.Take(period).Average() };

            for (int i = period; i < gains.Count; i++)
            {
                averageGains.Add((averageGains.Last() * (period - 1) + gains[i]) / period);
                averageLosses.Add((averageLosses.Last() * (period - 1) + losses[i]) / period);
            }

            List<decimal> RS = new List<decimal>();
            for (int i = 0; i < averageGains.Count; i++)
            {
                RS.Add(averageGains[i] / averageLosses[i]);
            }

            List<decimal> RSI = RS.Select(rs => 100 - (100 / (1 + rs))).ToList();

            for (int i = period; i < RSI.Count; i++)
            {
                if (RSI[i] < 30 && historicalData[i + 1].High > historicalData[i].Close + priceDifference)
                {
                    countOfHighsAfterOversoldByDifference++;
                }
            }

            return (double)countOfHighsAfterOversoldByDifference / (historicalData.Count - period);
        }

        public double CalculateNextHighProbabilityWithBollingerBands(List<BinanceKlineModel> historicalData, int period = 20, double multiplier = 2, decimal priceDifference = 0)
        {
            int countOfHighsAboveUpperBandByDifference = 0;

            for (int i = period; i < historicalData.Count - 1; i++)
            {
                var sma = historicalData.Skip(i - period).Take(period).Average(data => data.Close);
                var standardDeviation = Math.Sqrt(historicalData.Skip(i - period).Take(period).Average(data => Math.Pow((double)(data.Close - sma), 2)));

                decimal upperBand = sma + (decimal)(multiplier * (double)standardDeviation);

                if (historicalData[i + 1].High > upperBand + priceDifference)
                {
                    countOfHighsAboveUpperBandByDifference++;
                }
            }

            return (double)countOfHighsAboveUpperBandByDifference / (historicalData.Count - period);
        }

        public double CalculateNextHighProbabilityWithMACD(List<BinanceKlineModel> historicalData, int shortPeriod = 12, int longPeriod = 26, int signalPeriod = 9, decimal priceDifference = 0)
        {
            int countOfHighsAfterMACDCrossByDifference = 0;

            List<decimal> shortEma = ComputeEMA(historicalData.Select(data => data.Close).ToList(), shortPeriod);
            List<decimal> longEma = ComputeEMA(historicalData.Select(data => data.Close).ToList(), longPeriod);

            List<decimal> macdLine = shortEma.Zip(longEma, (shortVal, longVal) => shortVal - longVal).ToList();
            List<decimal> signalLine = ComputeEMA(macdLine, signalPeriod);

            for (int i = 1; i < signalLine.Count && i + longPeriod - 1 < macdLine.Count; i++)
            {
                if (macdLine[i + longPeriod - 1] > signalLine[i] && macdLine[i + longPeriod - 2] <= signalLine[i - 1] && historicalData[i + longPeriod].High > historicalData[i + longPeriod - 1].Close + priceDifference)
                {
                    countOfHighsAfterMACDCrossByDifference++;
                }
            }

            return (double)countOfHighsAfterMACDCrossByDifference / (historicalData.Count - longPeriod - signalPeriod + 1);
        }


        private List<decimal> ComputeEMA(List<decimal> prices, int period)
        {
            List<decimal> ema = new List<decimal>();
            decimal multiplier = 2.0M / (period + 1);
            ema.Add(prices.Take(period).Average());

            for (int i = period; i < prices.Count; i++)
            {
                decimal value = ((prices[i] - ema.Last()) * multiplier) + ema.Last();
                ema.Add(value);
            }

            return ema;
        }

        public double CalculateNextHighProbabilityWithStochasticOscillator(List<BinanceKlineModel> historicalData, int period = 14, int signalPeriod = 3, decimal priceDifference = 0)
        {
            int countOfHighsAfterStochasticCrossByDifference = 0;

            List<decimal> percentK = new List<decimal>();
            List<decimal> percentD = new List<decimal>();

            for (int i = period - 1; i < historicalData.Count; i++)
            {
                decimal highestHigh = historicalData.Skip(i + 1 - period).Take(period).Max(data => data.High);
                decimal lowestLow = historicalData.Skip(i + 1 - period).Take(period).Min(data => data.Low);
                decimal currentClose = historicalData[i].Close;

                decimal k = ((currentClose - lowestLow) / (highestHigh - lowestLow)) * 100;
                percentK.Add(k);
            }

            for (int i = 0; i < percentK.Count - signalPeriod + 1; i++)
            {
                decimal d = percentK.Skip(i).Take(signalPeriod).Average();
                percentD.Add(d);
            }

            for (int i = 1; i < percentD.Count; i++)
            {
                if (percentK[i] > percentD[i] && percentK[i - 1] <= percentD[i - 1] && historicalData[i + period].High > historicalData[i + period - 1].Close + priceDifference)
                {
                    countOfHighsAfterStochasticCrossByDifference++;
                }
            }

            return (double)countOfHighsAfterStochasticCrossByDifference / (historicalData.Count - period - signalPeriod + 1);
        }

        public double CalculateNextHighProbabilityWithParabolicSAR(List<BinanceKlineModel> historicalData, double startAF = 0.02, double incrementAF = 0.02, double maxAF = 0.2, decimal priceDifference = 0)
        {
            int countOfHighsAboveSARByDifference = 0;

            bool isUptrend = true;
            decimal extremePoint = historicalData[0].High;
            decimal accelerationFactor = (decimal)startAF;
            decimal sar = historicalData[0].Low;

            for (int i = 1; i < historicalData.Count; i++)
            {
                if (isUptrend)
                {
                    if (historicalData[i].Low <= sar)
                    {
                        isUptrend = false;
                        sar = extremePoint;
                        extremePoint = historicalData[i].Low;
                        accelerationFactor = (decimal)startAF;
                    }
                    else
                    {
                        if (historicalData[i].High > extremePoint)
                        {
                            extremePoint = historicalData[i].High;
                            accelerationFactor = Math.Min(accelerationFactor + (decimal)incrementAF, (decimal)maxAF);
                        }
                        sar += accelerationFactor * (extremePoint - sar);
                    }
                }
                else
                {
                    if (historicalData[i].High >= sar)
                    {
                        isUptrend = true;
                        sar = extremePoint;
                        extremePoint = historicalData[i].High;
                        accelerationFactor = (decimal)startAF;
                    }
                    else
                    {
                        if (historicalData[i].Low < extremePoint)
                        {
                            extremePoint = historicalData[i].Low;
                            accelerationFactor = Math.Min(accelerationFactor + (decimal)incrementAF, (decimal)maxAF);
                        }
                        sar -= accelerationFactor * (sar - extremePoint);
                    }
                }

                if (isUptrend && historicalData[i].Close > sar + priceDifference)
                {
                    countOfHighsAboveSARByDifference++;
                }
            }
            return (double)countOfHighsAboveSARByDifference / historicalData.Count;
        }

        public double CalculateCompositeProbability(List<BinanceKlineModel> historicalData, decimal priceDifference = 0)
        {
            Dictionary<string, double> weights = new Dictionary<string, double>
            {
                {"Basic", 0.15},
                {"Volume", 0.15},
                {"SMA", 0.15},
                {"RSI", 0.1},
                {"BollingerBands", 0.1},
                {"MACD", 0.05},
                {"StochasticOscillator", 0.1},
                {"ParabolicSAR", 0.2}
            };

            var basicProb = CalculateNextHighProbability(historicalData, priceDifference);
            var volumeProb = CalculateNextHighProbabilityWithVolume(historicalData, priceDifference);
            var smaProb = CalculateNextHighProbabilityWithSMA(historicalData, period: 10, priceDifference);
            var rsiProb = CalculateNextHighProbabilityWithRSI(historicalData, period: 14, priceDifference);
            var bollingerBandsProb = CalculateNextHighProbabilityWithBollingerBands(historicalData, priceDifference: priceDifference);
            var macdProb = CalculateNextHighProbabilityWithMACD(historicalData, priceDifference: priceDifference);
            var stochasticOscillatorProb = CalculateNextHighProbabilityWithStochasticOscillator(historicalData, priceDifference: priceDifference);
            var parabolicSARProb = CalculateNextHighProbabilityWithParabolicSAR(historicalData, priceDifference: priceDifference);

            var compositeProbability =
                weights["Basic"] * basicProb +
                weights["Volume"] * volumeProb +
                weights["SMA"] * smaProb +
                weights["RSI"] * rsiProb +
                weights["BollingerBands"] * bollingerBandsProb +
                weights["MACD"] * macdProb +
                weights["StochasticOscillator"] * stochasticOscillatorProb +
                weights["ParabolicSAR"] * parabolicSARProb;

            return compositeProbability * 100;
        }
    }
}
