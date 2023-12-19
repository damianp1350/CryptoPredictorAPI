using System.ComponentModel.DataAnnotations;

namespace CryptoPredictorAPI.Models
{
    public class PredictedPriceModel
    {
        [Key]
        public int Id { get; set; }

        public double Price { get; set; }

        public DateTime PredictedAt { get; set; }
    }
}
