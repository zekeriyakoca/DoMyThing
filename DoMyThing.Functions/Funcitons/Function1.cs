using System.Net;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DoMyThing.Functions
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly ProcessorFactory _factory;
        private readonly IProcessor<DownloadSubtitleModel> _processor;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _factory = new ProcessorFactory();
            _processor = _factory.GetProcessor<DownloadSubtitleModel>();

        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = streamReader.ReadToEnd() ?? "";
            }
            var model = JsonConvert.DeserializeObject<DownloadSubtitleModel>(requestBody);

            #region Temp Code

            model = model ?? new DownloadSubtitleModel { SearchText = "matrix", LanguageCode = "tur" };

            #endregion

            if (model is null)
            {
                throw new ArgumentNullException("model in body is required!");
            }

            await _processor.ProcessAsync(model);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
