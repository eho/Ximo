using System;
using Ximo.Domain;

namespace Sample.DomainEvents
{
    public class AccountCreated : IDomainEvent
    {
        private AccountCreated()
        {
        }

        public AccountCreated(Guid accountId, string businessName, int accountNumber) : this()
        {
            AccountNumber = accountNumber;
            BusinessName = businessName;
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
        public string BusinessName { get; private set; }
        public int AccountNumber { get; private set; }
    }
}