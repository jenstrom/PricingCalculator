using PricingCalculator.Data;
using PricingCalculator.Data.Models;
using PricingCalculator.Models;
using PricingCalculator.Models.Calculation;

namespace PricingCalculator.Calculation
{
    public class CalculationService : ICalculationService
    {
        private readonly IRepository _repository;

        public CalculationService(IRepository repository)
        {
            _repository = repository;
        }

        public decimal Calculate(CalculationRequest request)
        {
            var customer = _repository.GetCustomer(request.CustomerId);
            if (customer == null)
            {
                throw new ArgumentOutOfRangeException(nameof(request.CustomerId));
            }
            var startDate = request.StartDate.Date.AddDays(customer.FreeDays);
            var endDate = request.EndDate.Date;
            if (startDate >= endDate)
            {
                return 0;
            }
            return customer.Services.Where(s => s.Start.Date <= endDate).Select(s => new
            {
                ServiceDays = s.Service.DaysAvailable,
                StartDate = s.Start.Date > startDate ? s.Start.Date : startDate,
                Cost = customer.CustomPrices.FirstOrDefault(cp => cp.Service.Id == s.Service.Id)?.Price ?? s.Service.Cost,
                ApplicableDiscounts = customer.Discounts
                    .Where(
                        d => d.Service.Id == s.Service.Id
                        && d.Start.Date <= endDate && (d.End == null || d.End.Value.Date >= startDate))
                    .OrderByDescending(d => d.PercentageDiscount)
            }).SelectMany(x => GetDaysInRange(x.StartDate, endDate).Where(d => x.ServiceDays.Contains(d.DayOfWeek)).Select(
                d => (100 - x.ApplicableDiscounts.FirstOrDefault(dis => DateIsInRange(d.Date, dis.Start.Date, dis.End?.Date))?.PercentageDiscount) * x.Cost / 100 ?? x.Cost)
            ).Sum();
        }

        private bool DateIsInRange(DateTime date, DateTime start, DateTime? end) 
            => date >= start && (end == null || date <= end.Value);

        private IEnumerable<DateTime> GetDaysInRange(DateTime start, DateTime end)
        {
            for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}