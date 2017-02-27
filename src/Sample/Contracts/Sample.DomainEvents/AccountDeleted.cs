using System;
using Ximo.Domain;

namespace Sample.DomainEvents
{
    public class AccountDeleted : IDomainEvent
    {
        private AccountDeleted()
        {
        }

        public AccountDeleted(Guid accountId, string reason)
        {
            AccountId = accountId;
            Reason = reason;
        }

        public Guid AccountId { get; private set; }
        public string Reason { get; private set; }
    }
}