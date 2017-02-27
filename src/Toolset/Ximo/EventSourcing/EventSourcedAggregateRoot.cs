using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Ximo.Domain;
using Ximo.Utilities;

namespace Ximo.EventSourcing
{
    /// <summary>
    ///     Base class for an event sourced aggregate.
    /// </summary>
    public abstract class EventSourcedAggregateRoot : Entity<Guid>
    {
        private readonly Queue<DomainEventEnvelope> _uncommittedEvents;
        private bool _isDirty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventSourcedAggregateRoot" /> class.
        /// </summary>
        protected EventSourcedAggregateRoot()
        {
            _uncommittedEvents = new Queue<DomainEventEnvelope>();
            Version = 0;
            LastEventSequence = 0;
        }

        /// <summary>
        ///     Gets or sets the last event sequence.
        /// </summary>
        /// <value>The last event sequence.</value>
        public int LastEventSequence { get; protected internal set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; protected set; }

        /// <summary>
        ///     Gets the uncommitted events.
        /// </summary>
        /// <value>The uncommitted events.</value>
        public ReadOnlyCollection<DomainEventEnvelope> UncommittedEvents
            => new ReadOnlyCollection<DomainEventEnvelope>(_uncommittedEvents.ToList());

        /// <summary>
        ///     Marks the aggregate root as committed. Clears all uncommitted events.
        /// </summary>
        public void MarkAsCommitted()
        {
            _uncommittedEvents.Clear();
        }

        /// <summary>
        ///     Replays the specified historical events.
        /// </summary>
        /// <param name="historicalEvents">The historical events.</param>
        public void Replay(IEnumerable<DomainEventEnvelope> historicalEvents)
        {
            var orderedEventWrappers = historicalEvents.OrderBy(x => x.Sequence);
            foreach (var eventWrapper in orderedEventWrappers)
            {
                ApplyEvent(eventWrapper.Event);
                LastEventSequence = eventWrapper.Sequence;
                Version = eventWrapper.AggregateVersion;
            }
        }

        /// <summary>
        ///     Applies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        protected void ApplyChange(IDomainEvent @event)
        {
            ApplyEvent(@event);

            if (!_isDirty)
            {
                Version = ++Version;
                _isDirty = true;
            }

            var domainEventEnvelope = new DomainEventEnvelope(Id, ++LastEventSequence, Version, @event);
            _uncommittedEvents.Enqueue(domainEventEnvelope);
        }

        private void ApplyEvent(IDomainEvent @event)
        {
            try
            {
                this.AsDynamic().Apply(@event);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException != null)
                {
                    throw targetInvocationException.InnerException;
                }
                throw;
            }
        }
    }
}