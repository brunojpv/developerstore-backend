using DeveloperStore.Application.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DeveloperStore.Infrastructure.Events
{
    public class FakeEventPublisher : IEventPublisher
    {
        private readonly ILogger<FakeEventPublisher> _logger;

        public FakeEventPublisher(ILogger<FakeEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event)
        {
            var json = JsonSerializer.Serialize(@event);
            _logger.LogInformation("[EVENTO SIMULADO] Tipo: {EventType}, Conteúdo: {Json}", typeof(T).Name, json);
            return Task.CompletedTask;
        }
    }
}
