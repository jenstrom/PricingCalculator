using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Moq;
using PricingCalculator.Data;
using PricingCalculator.Data.Models;

namespace PricingCalculator.Calculation.Tests.CalculationServiceTests
{
    public class CalculateTests
    {
        private readonly CalculationService _sut;
        private readonly List<ServiceModel> _services;

        public CalculateTests()
        {
            IServiceRepository serviceRepository = Mock.Of<IServiceRepository>();
            _services = new List<ServiceModel>
            {
                new ServiceModel
                {
                    Name = Guid.NewGuid().ToString(),
                    Cost = 1,
                    DaysAvailable = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday }
                }
            };
            Mock.Get(serviceRepository).Setup(x => x.GetServices()).Returns(() => _services);
            Mock.Get(serviceRepository).Setup(x => x.GetServiceModel(_services[0].Name)).Returns(() => _services[0]);
            Mock.Get(serviceRepository).Setup(x => x.ServicesExist(It.IsAny<IEnumerable<string>>())).Returns(true);
            _sut = new CalculationService(serviceRepository);
        }

        [Fact]
        public void It_should_work_if_start_in_start_of_service_days()
        {
            var start = DateTime.ParseExact("2022-09-26", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(7);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count();
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_start_in_middle_of_service_days()
        {
            var start = DateTime.ParseExact("2022-09-28", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(7);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count();
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_start_after_service_days()
        {
            var start = DateTime.ParseExact("2022-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(7);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count();
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_span_is_several_weeks()
        {
            var weeks = 3;
            var start = DateTime.ParseExact("2022-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(weeks * 7);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count() * weeks;
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_span_is_not_full_weeks_and_start_monday()
        {
            var weeks = 3;
            var extraDays = 2;
            var start = DateTime.ParseExact("2022-09-26", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(weeks * 7 + extraDays);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count() * weeks + extraDays;
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_span_is_not_full_weeks_and_start_after_service_days()
        {
            var weeks = 3;
            var extraDays = 2;
            var start = DateTime.ParseExact("2022-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(weeks * 7 + extraDays);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count() * weeks;
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_work_if_service_days_are_split_up()
        {
            _services[0].DaysAvailable = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Friday, DayOfWeek.Saturday };
            var weeks = 3;
            var extraDays = 2;
            var start = DateTime.ParseExact("2022-09-26", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(weeks * 7 + extraDays);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>(),
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count() * weeks + extraDays;
            var result = _sut.Calculate(input);
            Assert.Equal(expected, result.Total);
        }

        [Fact]
        public void It_should_return_a_list_of_applied_discounts()
        {
            var weeks = 3;
            var start = DateTime.ParseExact("2022-09-26", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = start.AddDays(weeks * 7);
            var input = new List<CalculationRequest>
            {
                new CalculationRequest
                {
                    ServiceName = _services[0].Name,
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            Start = start.AddDays(-14),
                            End = start.AddDays(-7),
                            PercentageDiscount = 1
                        },
                        new Discount
                        {
                            Start = start.AddDays(3),
                            End = start.AddDays(10),
                            PercentageDiscount = 2
                        },
                        new Discount
                        {
                            Start = start.AddDays(12),
                            End = start.AddDays(20),
                            PercentageDiscount = 3
                        }
                    },
                    StartDate = start,
                    EndDate = end
                }
            };
            var expected = _services[0].Cost * _services[0].DaysAvailable.Count() * weeks;
            var result = _sut.Calculate(input);
            Assert.Collection(result.Calculations[0].AppliedDiscounts,
                item => Assert.Equal(2, item.PercentageDiscount),
                item => Assert.Equal(3, item.PercentageDiscount));
        }
    }
}
