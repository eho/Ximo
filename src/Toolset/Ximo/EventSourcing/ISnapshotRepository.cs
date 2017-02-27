using System;
using Ximo.Domain;

namespace Ximo.EventSourcing
{
    /// <summary>
    ///     Defines contract for a snapshot repository. Snapshots are generated to ease the load effort when an aggregate has a
    ///     considerable number of events.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
    /// <seealso cref="IRepository" />
    public interface ISnapshotRepository<TAggregateRoot> : IRepository where TAggregateRoot : EventSourcedAggregateRoot
    {
        /// <summary>
        ///     Gets the snapshot interval.
        /// </summary>
        /// <value>The snapshot interval.</value>
        int SnapshotInterval { get; }

        /// <summary>
        ///     Saves the snapshot.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        void SaveSnapshot(TAggregateRoot aggregateRoot);

        /// <summary>
        ///     Gets the latest snapshot.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>A previously snapshotted instance of the aggregate.</returns>
        Tuple<TAggregateRoot, int> GetLatestSnapshot(Guid aggregateId);
    }
}