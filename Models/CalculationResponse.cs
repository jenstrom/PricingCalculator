using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingCalculator.Models
{
    public class CalculationResponse
    {
        public List<CalculationResult> Calculations { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class CalculationResult
    {
        public decimal Total { get; set; }
        public List<Discount> AppliedDiscounts { get; set; } = new();
    }
}
