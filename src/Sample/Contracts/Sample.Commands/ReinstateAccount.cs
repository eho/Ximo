using System;
using Ximo.Cqrs;

namespace Sample.Commands
{
    public class ReinstateAccount : ICommand
    {
        private ReinstateAccount()
        {
        }

        public ReinstateAccount(Guid accountId) : this()
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}