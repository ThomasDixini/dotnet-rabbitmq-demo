using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Contracts.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : class;
    }
}