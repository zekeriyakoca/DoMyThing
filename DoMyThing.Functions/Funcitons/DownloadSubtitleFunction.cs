using DoMyThing.Common.Services.Interfaces;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DoMyThing.Functions
{
    public class DownloadSubtitleFunction
    {
        private readonly ILogger logger;
        private readonly IServiceBusService serviceBusService;

        private readonly IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel> processor;
        private readonly IConfiguration configuration;
        private readonly string subtitleDownloadedQueueName;

        public DownloadSubtitleFunction(ILoggerFactory loggerFactory,
            IServiceBusService serviceBusService,
            IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel> processor,
            IConfiguration configuration)
        {
            logger = loggerFactory.CreateLogger<DownloadSubtitleFunction>();
            this.serviceBusService = serviceBusService;
            this.processor = processor;
            this.configuration = configuration;
            subtitleDownloadedQueueName = configuration["SubtitleDownloadedQueueName"] ?? throw new ArgumentException("'Subtitle Downloaded Queue Name' cannot be null!");
        }

        [Function(nameof(DownloadSubtitleFunction))]
        public async Task Run(
            [ServiceBusTrigger("%DownloadSubtitleQueueName%", Connection = "ServiceBusConnection")] string message
            )
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            if (String.IsNullOrWhiteSpace(message))
            {
                logger.LogError("Message cannot be null!");
                return;
            }

            var model = JsonConvert.DeserializeObject<DownloadSubtitleModel>(message);

            if (model is null)
            {
                throw new ArgumentNullException("model in body is required!");
            }

            var processResponse = await processor.ProcessAsync(model);

            await serviceBusService.SendAsync(subtitleDownloadedQueueName, JsonConvert.SerializeObject(processResponse));
        }
    }
}
