using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Models;
using PricingCalculator.Calculation;
using PricingCalculator.Data;
using PricingCalculator.Models;

namespace PricingCalculator.Functions.Functions
{
    public class Services
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<Services> _logger;

        public Services(IServiceRepository serviceRepository, ILogger<Services> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }


        [Function(nameof(Get))]
        [OpenApiOperation(operationId: nameof(Get), tags: new[] { nameof(Services) })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(ServiceResponse[]), Description = "A list of available services.")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, nameof(Get), Route = nameof(Services))] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(Services)}_{nameof(Get)} processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            var services = _serviceRepository.GetServices().Select(s => new ServiceResponse
            {
                Name = s.Name,
                Cost = s.Cost,
                DaysAvailable = s.DaysAvailable
            }).ToArray();
            //var services = new ServiceResponse[]
            //{
            //    new ServiceResponse
            //    {
            //        Name = "test",
            //        Cost = 2,
            //        DaysAvailable = new List<DayOfWeek> { DayOfWeek.Monday }
            //    }
            //};
            await response.WriteAsJsonAsync(services);
            return response;
        }
    }
}
