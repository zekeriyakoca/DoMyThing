using DoMyThing.Common.Services.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services
{
    public class ServiceBusService : IServiceBusService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ServiceBusService> logger;
        private readonly string connectionString;

        private readonly ConcurrentDictionary<string, QueueClient> clients;

        public ServiceBusService(IConfiguration configuration, ILogger<ServiceBusService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration["ServiceBusConnection"] ?? throw new ArgumentNullException("ServiceBus connectionstring cannot be null!");
            clients = new();
        }

        public async Task SendAsync(string queueName, string message)
        {
            var client = GetQueueClient(queueName);
            await client.SendAsync(new Message(Encoding.UTF8.GetBytes(message)));
        }

        private QueueClient GetQueueClient(string queueName)
        {
            return clients.GetOrAdd(queueName, new QueueClient(connectionString, queueName));
        }

    }
}
