namespace Ximo.Cqrs
{
    /// <summary>
    ///     Represents a generic event handler.
    /// </summary>
    /// <typeparam name="TSystemEvent">The type of the event.</typeparam>
    public interface ISystemEventHandler<in TSystemEvent>
        where TSystemEvent : ISystemEvent
    {
        /// <summary>
        ///     Handles the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        void Handle(TSystemEvent @event);
    }
}