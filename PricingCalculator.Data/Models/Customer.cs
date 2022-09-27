namespace PricingCalculator.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int FreeDays { get; set; }
        public List<CustomerServiceModel> Services { get; set; } = new();
        public List<Discount> Discounts { get; set; } = new();
        public List<CustomPrice> CustomPrices { get; set; } = new();
    }
}
