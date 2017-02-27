namespace Ximo.Cqrs.Decorators
{
    /// <summary>
    ///     Defines a decorator for system event handlers. Each decorator provides the ability to add additional functionality
    ///     to query handlers while not violating the single responsibility principle or the open/closed principle for the
    ///     S.O.L.I.D design patterns.
    /// </summary>
    /// <typeparam name="TSystemEvent">The type of the system event.</typeparam>
    /// <seealso cref="ISystemEventHandler{TSystemEvent}" />
    public interface ISystemEventDecorator<in TSystemEvent> : ISystemEventHandler<TSystemEvent>
        where TSystemEvent : class, ISystemEvent
    {
    }
}