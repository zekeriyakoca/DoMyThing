using DoMyThing.Common.Services;
using DoMyThing.Common.Services.Interfaces;
using DoMyThing.Functions;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using DoMyThing.Functions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddHttpClient();
        s.AddTransient<IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel>, DownloadSubtitleProcessor>();
        s.AddScoped<IServiceBusService, ServiceBusService>();
        s.AddSingleton<IBlobStorageByteService, BlobStorageService>();
        s.AddSingleton<SubtitleStorageAppService>();
    })
    .Build();

host.Run();
