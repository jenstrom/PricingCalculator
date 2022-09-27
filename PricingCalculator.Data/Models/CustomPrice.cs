namespace PricingCalculator.Data.Models
{
    public class CustomPrice
    {
        public ServiceModel Service { get; set; } = new();
        public decimal Price { get; set; }
    }
}
