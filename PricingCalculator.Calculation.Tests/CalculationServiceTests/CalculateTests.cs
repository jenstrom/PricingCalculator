using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Moq;
using PricingCalculator.Data;
using PricingCalculator.Data.Models;
using PricingCalculator.Models.Calculation;

namespace PricingCalculator.Calculation.Tests.CalculationServiceTests
{
    public class CalculateTests
    {
        private readonly CalculationService _sut;

        public CalculateTests()
        {
            IRepository repository = Mock.Of<IRepository>();
            Mock.Get(repository).Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(new Customer
            {
                Services = new List<CustomerServiceModel>
                {
                    new CustomerServiceModel
                    {
                        Start = DateTime.Now,
                        Service = new ServiceModel
                        {
                            Id = 1,
                            Cost = 1,
                            DaysAvailable = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }
                        }
                    }
                },
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(365),
                        PercentageDiscount = 20,
                        Service = new ServiceModel
                        {
                            Id = 1
                        }
                    },
                    new Discount
                    {
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(365*3),
                        PercentageDiscount = 10,
                        Service = new ServiceModel
                        {
                            Id = 1
                        }
                    },
                    new Discount
                    {
                        Start = DateTime.Now.AddDays(365),
                        End = DateTime.Now.AddDays(365*2),
                        PercentageDiscount = 30,
                        Service = new ServiceModel
                        {
                            Id = 1
                        }
                    },
                    new Discount
                    {
                        Start = DateTime.Now.AddDays(365*2),
                        End = DateTime.Now.AddDays(365*3),
                        PercentageDiscount = 40,
                        Service = new ServiceModel
                        {
                            Id = 1
                        }
                    }
                }
            });
            _sut = new CalculationService(repository);
        }

        [Fact]
        public void It_Should()
        {
            var result = _sut.Calculate(new CalculationRequest
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                CustomerId = 1
            });
        }

    }
}
