namespace PricingCalculator.Data.Models
{
    public class CustomerServiceModel
    {
        public DateTime Start { get; set; }
        public Customer Customer { get; set; } = new();
        public ServiceModel Service { get; set; } = new();
    }
}
