using PricingCalculator.Models;
using PricingCalculator.Models.Calculation;

namespace PricingCalculator.Calculation
{
    public interface ICalculationService
    {
        decimal Calculate(CalculationRequest request);
    }
}