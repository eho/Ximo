namespace Ximo.Domain
{
    /// <summary>
    ///     Represents a domain event handler.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of the event.</typeparam>
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        /// <summary>
        ///     Handles the specified domain event.
        /// </summary>
        /// <param name="event">The domain event.</param>
        void Handle(TDomainEvent @event);
    }
}