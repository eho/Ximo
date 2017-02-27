using System.Threading.Tasks;

namespace Ximo.Domain
{
    /// <summary>
    ///     Represents a generic event bus.
    /// </summary>
    public interface IDomainEventBus
    {
        /// <summary>
        ///     Creates a subscription to the specified event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the event.</typeparam>
        /// <typeparam name="TConcreteHandler">The concrete type of the event handler.</typeparam>
        void Subscribe<TDomainEvent, TConcreteHandler>() where TDomainEvent : class, IDomainEvent
            where TConcreteHandler : IDomainEventHandler<TDomainEvent>;

        /// <summary>
        ///     Removes a subscription to the specified event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the event.</typeparam>
        /// <typeparam name="TConcreteHandler">The concrete type of the event handler.</typeparam>
        void UnSubscribe<TDomainEvent, TConcreteHandler>() where TDomainEvent : class, IDomainEvent
            where TConcreteHandler : IDomainEventHandler<TDomainEvent>;

        /// <summary>
        ///     Publishes the specified event. Delivers the event to the registered event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        void Publish<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class, IDomainEvent;

        /// <summary>
        ///     Publishes the specified event asynchronously. Delivers the event to the registered event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        Task PublishAsync<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class, IDomainEvent;
    }
}