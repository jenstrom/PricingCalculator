using PricingCalculator.Data.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace PricingCalculator.Data
{
    public class Repository : IRepository
    {
        private readonly List<ServiceModel> _services;
        private readonly List<Customer> _customers;

        public Repository()
        {
            _services = new List<ServiceModel>
            {
                new ServiceModel
                {
                    Id = 1,
                    Name = "ServiceA",
                    Cost = 0.2M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }
                },
                new ServiceModel
                {
                    Id = 2,
                    Name = "ServiceB",
                    Cost = 0.24M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }
                },
                new ServiceModel
                {
                    Id = 3,
                    Name = "ServiceC",
                    Cost = 0.4M,
                    DaysAvailable  = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }
                }
            };
            _customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Customer X",
                    Services = new List<CustomerServiceModel>(),
                    CustomPrices = new List<CustomPrice>(),
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            Start = DateTime.ParseExact("2019-09-22", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            End = DateTime.ParseExact("2019-09-24", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            PercentageDiscount = 20,
                            Service = _services.First(s => s.Id == 3)
                        }
                    },
                    FreeDays = 0
                },
                new Customer
                {
                    Id = 2,
                    Name = "Customer Y",
                    Services = new List<CustomerServiceModel>(),
                    CustomPrices = new List<CustomPrice>(),
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            Start = DateTime.ParseExact("2018-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            End = null,
                            PercentageDiscount = 30,
                            Service = _services.First(s => s.Id == 2)
                        },
                        new Discount
                        {
                            Start = DateTime.ParseExact("2018-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            End = null,
                            PercentageDiscount = 30,
                            Service = _services.First(s => s.Id == 3)
                        }
                    },
                    FreeDays = 200
                }
            };
            var customerServices = new List<CustomerServiceModel>
            {
                new CustomerServiceModel
                {
                    Start = DateTime.ParseExact("2019-09-20", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Customer = _customers.First(c => c.Id == 1),
                    Service = _services.First(s => s.Id == 1)
                },
                new CustomerServiceModel
                {
                    Start = DateTime.ParseExact("2019-09-20", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Customer = _customers.First(c => c.Id == 1),
                    Service = _services.First(s => s.Id == 3)
                },
                new CustomerServiceModel
                {
                    Start = DateTime.ParseExact("2018-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Customer = _customers.First(c => c.Id == 2),
                    Service = _services.First(s => s.Id == 2)
                },
                new CustomerServiceModel
                {
                    Start = DateTime.ParseExact("2018-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Customer = _customers.First(c => c.Id == 2),
                    Service = _services.First(s => s.Id == 3)
                }
            };
            foreach (var customerService in customerServices)
            {
                _customers.First(c => c.Id == customerService.Customer.Id).Services.Add(customerService);
                _services.First(s => s.Id == customerService.Service.Id).Customers.Add(customerService); 
            }
        }

        public List<ServiceModel> GetServices() => _services;
        public List<Customer> GetCustomers() => _customers;

        public Customer? GetCustomer(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }
    }
}