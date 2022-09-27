using Models;
using PricingCalculator.Models;

namespace PricingCalculator.Calculation
{
    public interface ICalculationService
    {
        CalculationResponse Calculate(IEnumerable<CalculationRequest> requests);
    }
}