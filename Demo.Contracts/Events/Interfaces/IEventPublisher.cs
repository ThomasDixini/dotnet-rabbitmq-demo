namespace Demo.Contracts.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : class;
    }
}