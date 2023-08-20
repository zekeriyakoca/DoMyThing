using System.Net;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DoMyThing.Functions
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly ProcessorFactory _factory;
        private readonly IProcessor<Model1> _processor;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _factory = new ProcessorFactory();
            _processor = _factory.GetProcessor<Model1>();

        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // TODO : Read model from body
            var model = new Model1();
            _processor.Process(model);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
