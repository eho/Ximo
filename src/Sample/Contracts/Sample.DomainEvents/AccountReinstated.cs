using System;
using Ximo.Domain;

namespace Sample.DomainEvents
{
    public class AccountReinstated : IDomainEvent
    {
        private AccountReinstated()
        {
        }

        public AccountReinstated(Guid accountId) : this()
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}