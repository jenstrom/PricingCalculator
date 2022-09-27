namespace PricingCalculator.Models.Calculation
{
    public class CalculationResponse
    {
        public List<CalculationResult> Calculations { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class CalculationResult
    {
        public decimal Total { get; set; }
    }
}
