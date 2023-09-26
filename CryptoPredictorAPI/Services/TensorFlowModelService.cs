using Microsoft.ML;
using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
{
    public class TensorFlowModelService : ITensorFlowModelService
    {
        private readonly ITransformer _model;
        private readonly MLContext _mlContext;

        public TensorFlowModelService()
        {
            _mlContext = new MLContext();

            var tensorFlowModel = _mlContext.Model.LoadTensorFlowModel(@"path_to_model.pb");

            var inputColumnName = "placeholder";
            var outputColumnName = "placeholder";

            var dummyData = _mlContext.Data.LoadFromEnumerable(new List<BitcoinPriceInput> { new BitcoinPriceInput() });

            var tensorflowEstimator = tensorFlowModel.ScoreTensorFlowModel(outputColumnName: outputColumnName,
                                                                           inputColumnName: inputColumnName,
                                                                           addBatchDimensionInput: true);
            _model = tensorflowEstimator.Fit(dummyData);
        }

        public BitcoinPriceOutput Predict(BitcoinPriceInput input)
        {
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<BitcoinPriceInput, BitcoinPriceOutput>(_model);
            return predictionFunction.Predict(input);
        }
    }
}
