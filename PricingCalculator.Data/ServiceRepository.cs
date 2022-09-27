using PricingCalculator.Data.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PricingCalculator.Data
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly List<ServiceModel> _services;

        public ServiceRepository()
        {
            _services = new List<ServiceModel>
            {
                new ServiceModel
                {
                    Name = "ServiceA",
                    Cost = 0.2M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }
                },
                new ServiceModel
                {
                    Name = "ServiceB",
                    Cost = 0.24M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }
                },
                new ServiceModel
                {
                    Name = "ServiceC",
                    Cost = 0.4M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }
                }
            };
        }

        public List<ServiceModel> GetServices() => _services;

        public ServiceModel? GetServiceModel(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentException("Value must not be null or empty", nameof(serviceName));
            }
            return _services.FirstOrDefault(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
        }

        public bool ServicesExist(IEnumerable<string> serviceNames)
        {
            var services = GetServices();
            if (serviceNames == null || !serviceNames.Any())
            {
                throw new ArgumentException("Collection must not be null or empty", nameof(serviceNames));
            }
            if (serviceNames.GroupBy(s => s).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Collection must not contain duplicate values", nameof(serviceNames));
            }
            return services.Where(s => serviceNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)).GroupBy(s => s.Name).Count() == serviceNames.Count();
        }
    }
}