using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingCalculator.Data.Models
{
    public class ServiceModel
    {
        public string Name { get; set; } = "";
        public decimal Cost { get; set; }
        public List<DayOfWeek> DaysAvailable { get; set; } = new();
    }
}
