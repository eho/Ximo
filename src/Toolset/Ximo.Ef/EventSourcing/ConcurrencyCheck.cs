using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ximo.EventSourcing;

namespace Ximo.Ef.EventSourcing
{
    internal static class ConcurrencyCheck<TAggregateRoot, TEventSet>
        where TAggregateRoot : EventSourcedAggregateRoot
        where TEventSet : EfDomainEvent
    {
        public static void Process(TAggregateRoot aggregateRoot, DbSet<TEventSet> dbSet)
        {
            var persistedVersion =
                dbSet.Where(x => x.AggregateId == aggregateRoot.Id)
                    .Select(x => x.AggregateVersion)
                    .DefaultIfEmpty(0)
                    .Max();

            if (aggregateRoot.Version != persistedVersion + 1)
            {
                string errorMessage =
                    $"The aggregate with aggregate root of type {aggregateRoot.GetType().Name} has been modified and the event stream cannot be appended.";
                throw new AggregateConcurrencyException(errorMessage);
            }
        }
    }
}