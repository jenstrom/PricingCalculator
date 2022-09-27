namespace PricingCalculator.Data.Models
{
    public class Discount
    {
        public ServiceModel Service { get; set; } = new();
        public int PercentageDiscount { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
