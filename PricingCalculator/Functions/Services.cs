using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using PricingCalculator.Data;
using PricingCalculator.Models.Service;

namespace PricingCalculator.Functions.Functions
{
    public class Services
    {
        private readonly IRepository _repository;
        private readonly ILogger<Services> _logger;

        public Services(IRepository repository, ILogger<Services> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [Function(nameof(Get))]
        [OpenApiOperation(operationId: nameof(Get), tags: new[] { nameof(Services) })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(ServiceResponse[]), Description = "A list of available services.")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, nameof(Get), Route = nameof(Services))] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(Services)}_{nameof(Get)} processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            var services = _repository.GetServices().Select(s => new ServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Cost = s.Cost,
                DaysAvailable = s.DaysAvailable
            }).ToArray();
            await response.WriteAsJsonAsync(services);
            return response;
        }
    }
}
