using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PricingCalculator.Calculation;
using PricingCalculator.Models.Calculation;

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
        [OpenApiRequestBody(contentType: MediaTypeNames.Application.Json, bodyType: typeof(CalculationRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(CalculationResponse))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, methods: nameof(HttpMethod.Post), Route = nameof(Calculate))] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(Calculate)}_{nameof(Post)} processed a request.");
            var json = req.ReadAsStringAsync().Result;
            var calculationRequest = JsonConvert.DeserializeObject<CalculationRequest>(json);
            if (calculationRequest == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            try
            {
                var result = decimal.Round(_calculationService.Calculate(calculationRequest), 2);
                await response.WriteStringAsync(result.ToString(), Encoding.UTF8);
                response.Headers.Add("Content-Type", MediaTypeNames.Text.Plain);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            return response;
        }
    }
}
