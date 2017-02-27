using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sample.Domain.Repositories;
using Sample.DomainEvents;
using Ximo.Domain;
using Ximo.EventSourcing;
using Ximo.Extensions;

namespace Sample.Domain.EventHandlers
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    internal class AccountReinstatedHandler : IDomainEventHandler<AccountReinstated>
    {
        private readonly IAccountStore _accountStore;
        private readonly IDomainEventBus _domainEventBus;

        public AccountReinstatedHandler(IAccountStore accountStore, IDomainEventBus domainEventBus)
        {
            _accountStore = accountStore;
            _domainEventBus = domainEventBus;
        }

        public void Handle(AccountReinstated @event)
        {
            //SHOWS A CONCRETE EXAMPLE OF EVENT REPLAY

            var events = _accountStore.GetAggregateEvents(@event.AccountId);

            var reinstatingStream = BuilderReinstatingStream(events);

            foreach (var e in reinstatingStream)
            {
                _domainEventBus.Publish(e.Event);
            }
        }

        private static List<DomainEventEnvelope> BuilderReinstatingStream(IEnumerable<DomainEventEnvelope> events)
        {
            //Filter out the last delete event and remove approval and reinstating events
            var eventsToBeIgnoredIds =
                events.Where(
                        e =>
                            e.EventName == typeof(AccountDeleted).FullName ||
                            e.EventName == typeof(AccountReinstated).FullName ||
                            e.EventName == typeof(AccountApproved).FullName)
                    .Select(e => e.EventId).ToList();

            List<DomainEventEnvelope> reinstatingStream;

            if (!eventsToBeIgnoredIds.IsNullOrEmpty())
            {
                var eventsInScope = events.Where(e => !eventsToBeIgnoredIds.Contains(e.EventId)).ToList();
                reinstatingStream = new List<DomainEventEnvelope>(eventsInScope);
            }
            else
            {
                reinstatingStream = new List<DomainEventEnvelope>(events);
            }
            return reinstatingStream;
        }
    }
}