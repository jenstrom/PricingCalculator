using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Models;
using PricingCalculator.Calculation;
using PricingCalculator.Models;

namespace PricingCalculator.Functions.Functions
{
    public class Calculate
    {
        private readonly ICalculationService _calculationService;
        private readonly ILogger<Calculate> _logger;

        public Calculate(ICalculationService calculationService, ILogger<Calculate> logger)
        {
            _calculationService = calculationService;
            _logger = logger;
        }

        [Function(nameof(Post))]
        [OpenApiOperation(operationId: nameof(Post), tags: new[] { nameof(Calculate) }, Description = "Description of this action.")]
        [OpenApiRequestBody(contentType: MediaTypeNames.Application.Json, bodyType: typeof(CalculationRequest[]))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(CalculationResponse))]
        public HttpResponseData Post([HttpTrigger(AuthorizationLevel.Anonymous, methods: nameof(HttpMethod.Post), Route = nameof(Calculate))] HttpRequestData req, CalculationRequest[] calculationData)
        {
            _logger.LogInformation($"{nameof(Calculate)}_{nameof(Post)} processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", MediaTypeNames.Application.Json);

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
