namespace Models
{
    public class CalculationRequest
    {
        public string ServiceName { get; set; } = "";
        public List<Discount> Discounts { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Discount
    {
        public int PercentageDiscount { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}