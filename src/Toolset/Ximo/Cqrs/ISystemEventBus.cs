using System.Threading.Tasks;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     Represents a generic event bus.
    /// </summary>
    public interface ISystemEventBus
    {
        /// <summary>
        ///     Creates a subscription to the specified event.
        /// </summary>
        /// <typeparam name="TSystemEvent">The type of the event.</typeparam>
        /// <typeparam name="TConcreteHandler">The concrete type of the event handler.</typeparam>
        void Subscribe<TSystemEvent, TConcreteHandler>() where TSystemEvent : class, ISystemEvent
            where TConcreteHandler : ISystemEventHandler<TSystemEvent>;

        /// <summary>
        ///     Removes a subscription to the specified event.
        /// </summary>
        /// <typeparam name="TSystemEvent">The type of the event.</typeparam>
        /// <typeparam name="TConcreteHandler">The concrete type of the event handler.</typeparam>
        void UnSubscribe<TSystemEvent, TConcreteHandler>() where TSystemEvent : class, ISystemEvent
            where TConcreteHandler : ISystemEventHandler<TSystemEvent>;

        /// <summary>
        ///     Publishes the specified event. Delivers the event to the registered event handler.
        /// </summary>
        /// <typeparam name="TSystemEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        void Publish<TSystemEvent>(TSystemEvent @event, bool throwWhenNotSubscribedTo = true)
            where TSystemEvent : class, ISystemEvent;

        /// <summary>
        ///     Publishes the specified event asynchronously. Delivers the event to the registered event handler.
        /// </summary>
        /// <typeparam name="TSystemEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        Task PublishAsync<TSystemEvent>(TSystemEvent @event, bool throwWhenNotSubscribedTo = true)
            where TSystemEvent : class, ISystemEvent;
    }
}