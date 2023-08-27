using DoMyThing.Common.Services;
using DoMyThing.Common.Services.Interfaces;
using DoMyThing.Functions;
using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddHttpClient();
        s.AddTransient<IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel>, DownloadSubtitleProcessor>();
        s.AddScoped<IBlobStorageByteService, BlobStorageService>();
    })
    .Build();

host.Run();
