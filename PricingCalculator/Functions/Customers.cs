using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using PricingCalculator.Data;
using PricingCalculator.Models.Customer;

namespace PricingCalculator.Functions.Functions
{
    public class Customers
    {
        private readonly IRepository _repository;
        private readonly ILogger<Customers> _logger;

        public Customers(IRepository repository, ILogger<Customers> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [Function(nameof(Customers) + nameof(Get))]
        [OpenApiOperation(operationId: nameof(Get), tags: new[] { nameof(Customers) })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(CustomerResponse[]), Description = "A list of available customers.")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, nameof(Get), Route = nameof(Customers))] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(Customers)}_{nameof(Get)} processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            var customers = _repository.GetCustomers().Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                FreeDays = c.FreeDays,
                Services = c.Services.Select(s => s.Service.Name).ToList(),
                CustomPrices = c.CustomPrices.Select(cp => new CustomPriceResponse
                {
                    Price = cp.Price,
                    ServiceName = cp.Service.Name
                }).ToList(),
                Discounts = c.Discounts.Select(d => new DiscountResponse
                {
                    ServiceName = d.Service.Name,
                    Start = d.Start,
                    End = d.End,
                    PercentageDiscount = d.PercentageDiscount
                }).ToList()
                
            }).ToArray();
            await response.WriteAsJsonAsync(customers);
            return response;
        }
    }
}
