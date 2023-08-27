namespace DoMyThing.Common.Services.Interfaces
{
    public interface IServiceBusService
    {
        Task SendAsync(string queueName, string message);
    }
}