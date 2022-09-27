namespace PricingCalculator.Models.Calculation
{
    public class CalculationRequest
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}