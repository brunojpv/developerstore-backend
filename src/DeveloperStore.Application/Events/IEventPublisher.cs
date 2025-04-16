namespace DeveloperStore.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event);
    }
}
