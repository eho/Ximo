using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ximo.EventSourcing;

namespace Ximo.Ef.EventSourcing
{
    public class EfSnapshotRepository<TAggregateRoot, TAggregateMemento, TContext> : EfRepository<TContext>,
        ISnapshotRepository<TAggregateRoot>
        where TAggregateRoot : EventSourcedAggregateRoot
        where TContext : DbContext
        where TAggregateMemento : class, IAggregateMemento
    {
        public EfSnapshotRepository(TContext context) : base(context)
        {
            SnapshotInterval = 10;
        }

        /// <summary>
        ///     Gets or sets the snapshot interval.
        /// </summary>
        /// <value>The snapshot interval.</value>
        public int SnapshotInterval { get; protected set; }

        public void SaveSnapshot(TAggregateRoot aggregateRoot)
        {
            var snapshot = (TAggregateMemento) Activator.CreateInstance(typeof(TAggregateMemento), aggregateRoot);
            var snapshotSet = Context.Set<TAggregateMemento>();
            snapshotSet.Add(snapshot);
        }

        public Tuple<TAggregateRoot, int> GetLatestSnapshot(Guid aggregateId)
        {
            var lastEventSequence = 0;

            var snapshotSet = Context.Set<TAggregateMemento>();
            var snapshot =
                snapshotSet.AsNoTracking()
                    .OrderByDescending(s => s.LastEventSequence)
                    .FirstOrDefault(x => x.AggregateRootId == aggregateId);

            if (snapshot == null)
            {
                return null;
            }

            lastEventSequence = snapshot.LastEventSequence;

            var stringSerializer = new JsonObjectSerializer();
            var aggregateRoot = stringSerializer.Deserialize<TAggregateRoot>(snapshot.Payload);
            return new Tuple<TAggregateRoot, int>(aggregateRoot, lastEventSequence);
        }
    }
}