using System;
using Ximo.Cqrs;

namespace Sample.Commands
{
    public class DeleteAccount : ICommand
    {
        private DeleteAccount()
        {
        }

        public DeleteAccount(Guid accountId, string reason) : this()
        {
            AccountId = accountId;
            Reason = reason;
        }

        public Guid AccountId { get; private set; }
        public string Reason { get; private set; }
    }
}