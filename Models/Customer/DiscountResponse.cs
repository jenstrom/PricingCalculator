namespace PricingCalculator.Models.Customer
{
    public class DiscountResponse
    {
        public string ServiceName { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int PercentageDiscount { get; set; }
    }
}
