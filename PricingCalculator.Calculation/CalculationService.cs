using Models;
using PricingCalculator.Data;
using PricingCalculator.Models;

namespace PricingCalculator.Calculation
{
    public class CalculationService : ICalculationService
    {
        private readonly IServiceRepository _serviceRepository;

        public CalculationService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public CalculationResponse Calculate(IEnumerable<CalculationRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                throw new ArgumentException("Collection must not be null or empty", nameof(requests));
            }
            if (!_serviceRepository.ServicesExist(requests.Select(r => r.ServiceName)))
            {
                throw new ArgumentException("Collection contains invalid service name", nameof(requests));
            }
            var calculations = requests.Select(CalculateRequest).ToList();
            return new CalculationResponse
            {
                Calculations = calculations,
                Total = calculations.Sum(c => c.Total)
            };
        }

        private CalculationResult CalculateRequest(CalculationRequest request)
        {
            var applicableDiscounts = request.Discounts.Where(d => d.End > d.Start && d.End > request.StartDate && d.Start < request.EndDate);
            var service = _serviceRepository.GetServiceModel(request.ServiceName);
            var paidDays = GetNumberOfActiveDaysBetweenDates(request.StartDate, request.EndDate, service.DaysAvailable);


            return new CalculationResult
            {
                Total = paidDays * service.Cost,
                AppliedDiscounts = applicableDiscounts.ToList()
            };
        }

        private void ApplyDiscounts(IEnumerable<Discount> discounts, int numberOfFullPaidDays, DateTime start, DateTime end)
        {
            var orderedDiscounts = discounts.OrderByDescending(d => d.PercentageDiscount).ToList();
            for (int i = 0; i < orderedDiscounts.Count; i++)
            {
                var discount = orderedDiscounts[i];
                var applicable = discount.Start <= discount.End && discount.Start <= end && discount.End > start;
                if (applicable)
                {
                    var discountStart = discount.Start < start ? start : discount.Start;
                    var discountEnd = discount.End > end ? end : discount.End;
                    var betterDiscounts = discounts.Take(i).ToList();
                    for (int j = 0; j < betterDiscounts.Count; j++)
                    {
                        var betterDiscount = betterDiscounts[j];

                    }
                }
            }

        }

        private int GetNumberOfActiveDaysBetweenDates(DateTime start, DateTime end, IEnumerable<DayOfWeek> activeDays)
        {
            var looseDaysAtStart = (7 + (int)end.DayOfWeek - (int)start.DayOfWeek) % 7;
            var numberOfFullWeeks = (end - start.AddDays(looseDaysAtStart)).Days / 7;
            var paidDays = numberOfFullWeeks * activeDays.Count();
            for (int i = 0; i < looseDaysAtStart; i++)
            {
                if (activeDays.Contains(start.AddDays(i).DayOfWeek))
                {
                    paidDays++;
                }
            }
            return paidDays;
        }
    }
}