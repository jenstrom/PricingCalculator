using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingCalculator.Models.Service
{
    public class ServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Cost { get; set; }
        public List<DayOfWeek> DaysAvailable { get; set; } = new();
    }
}
