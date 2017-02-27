using System;

namespace Ximo.EventSourcing
{
    /// <summary>
    ///     Defines the contract for an event sourced aggregate snapshot.
    /// </summary>
    public interface IAggregateMemento
    {
        /// <summary>
        ///     Gets the aggregate root identifier.
        /// </summary>
        /// <value>The aggregate root identifier.</value>
        Guid AggregateRootId { get; }

        /// <summary>
        ///     Gets the last event sequence.
        /// </summary>
        /// <value>The last event sequence.</value>
        int LastEventSequence { get; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <value>The version.</value>
        int Version { get; }

        /// <summary>
        ///     Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        string Payload { get; }
    }
}