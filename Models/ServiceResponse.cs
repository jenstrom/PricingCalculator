using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingCalculator.Models
{
    public class ServiceResponse
    {
        public string Name { get; set; } = "";
        public decimal Cost { get; set; }
        public List<DayOfWeek> DaysAvailable { get; set; } = new();
    }
}
