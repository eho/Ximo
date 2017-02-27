using Sample.Domain.Data.DataModel;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using Ximo.Domain;
using Ximo.Ef.EventSourcing;
using Ximo.EventSourcing;

namespace Sample.Domain.Data.Repositories
{
    internal class AccountStore : EfEventStore<Account, AccountEvent, DomainDataContext>, IAccountStore
    {
        public AccountStore(DomainDataContext context, IDomainEventBus domainEventBus,
            ISnapshotRepository<Account> snapshotRepository) : base(context, domainEventBus, snapshotRepository)
        {
        }
    }
}