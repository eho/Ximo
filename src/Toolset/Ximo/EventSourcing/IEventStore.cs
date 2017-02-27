using System;
using System.Collections.Generic;
using Ximo.Domain;

namespace Ximo.EventSourcing
{
    /// <summary>
    ///     Interface defining contracts used for aggregate domain events persistence.
    /// </summary>
    /// <seealso cref="IRepository" />
    public interface IEventStore<TAggregateRoot> : IRepository where TAggregateRoot : EventSourcedAggregateRoot
    {
        /// <summary>
        ///     Saves the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        void Save(TAggregateRoot aggregateRoot);

        /// <summary>
        ///     Gets the aggregate by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The instance of the persisted aggregate.</returns>
        TAggregateRoot GetById(Guid id);

        /// <summary>
        ///     Gets all previous aggregate events.
        /// </summary>
        /// <param name="aggregateRootId">The aggregate root identifier.</param>
        /// <returns>
        ///     A list of <see cref="DomainEventEnvelope" /> wrapping the events of the
        ///     <see cref="EventSourcedAggregateRoot" />.
        /// </returns>
        IEnumerable<DomainEventEnvelope> GetAggregateEvents(Guid aggregateRootId);

        /// <summary>
        ///     Gets all previous aggregate events starting from a specific sequence number.
        /// </summary>
        /// <param name="aggregateRootId">The aggregate root identifier.</param>
        /// <param name="startSequenceNumber">The sequence number to start loading from.</param>
        /// <returns>
        ///     A list of <see cref="DomainEventEnvelope" /> wrapping the events of the
        ///     <see cref="EventSourcedAggregateRoot" />.
        /// </returns>
        IEnumerable<DomainEventEnvelope> GetAggregateEvents(Guid aggregateRootId, int startSequenceNumber);

        /// <summary>
        ///     Gets the aggregate events of a specific type.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event by which to retrieve.</typeparam>
        /// <param name="aggregateRootId">The aggregate root identifier.</param>
        /// <returns>
        ///     A list of <see cref="DomainEventEnvelope" /> wrapping the events of the
        ///     <see cref="EventSourcedAggregateRoot" />.
        /// </returns>
        IEnumerable<DomainEventEnvelope> GetAggregateEvents<TDomainEvent>(Guid aggregateRootId)
            where TDomainEvent : IDomainEvent;

        /// <summary>
        ///     Gets the aggregate events that are specified within the supplied types.
        /// </summary>
        /// <param name="aggregateRootId">The aggregate root identifier.</param>
        /// <param name="domainEventTypes">The domain event types.</param>
        /// <returns>
        ///     A list of <see cref="DomainEventEnvelope" /> wrapping the events of the
        ///     <see cref="EventSourcedAggregateRoot" />.
        /// </returns>
        IEnumerable<DomainEventEnvelope> GetAggregateEvents(Guid aggregateRootId, IEnumerable<Type> domainEventTypes);
    }
}