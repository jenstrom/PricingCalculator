namespace PricingCalculator.Models.Customer
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int FreeDays { get; set; }
        public List<string> Services { get; set; } = new();
        public List<DiscountResponse> Discounts { get; set; } = new();
        public List<CustomPriceResponse> CustomPrices { get; set; } = new();
    }
}
