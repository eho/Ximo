using Sample.Domain.Entities;
using Ximo.EventSourcing;

namespace Sample.Domain.Repositories
{
    public interface IAccountStore : IEventStore<Account>
    {
    }
}