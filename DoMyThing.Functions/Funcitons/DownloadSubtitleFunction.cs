using System.Net;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DoMyThing.Functions
{
    public class DownloadSubtitleFunction
    {
        private readonly ILogger _logger;
        //private readonly ProcessorFactory _factory;
        private readonly IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel> _processor;

        public DownloadSubtitleFunction(ILoggerFactory loggerFactory, /* ProcessorFactory factory, */IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel> processor)
        {
            _logger = loggerFactory.CreateLogger<DownloadSubtitleFunction>();
            _processor = processor; // factory.GetProcessor<DownloadSubtitleModel>();

        }

        [Function(nameof(DownloadSubtitleFunction))]
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

            var processResponse = await _processor.ProcessAsync(model);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
