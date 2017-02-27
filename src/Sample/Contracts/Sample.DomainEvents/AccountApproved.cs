using System;
using Ximo.Domain;

namespace Sample.DomainEvents
{
    public class AccountApproved : IDomainEvent
    {
        private AccountApproved()
        {
        }

        public AccountApproved(Guid accountId, string approvedBy) : this()
        {
            AccountId = accountId;
            ApprovedBy = approvedBy;
        }

        public Guid AccountId { get; private set; }
        public string ApprovedBy { get; private set; }
    }
}