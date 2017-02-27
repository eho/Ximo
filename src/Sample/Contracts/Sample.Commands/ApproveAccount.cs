using System;
using Ximo.Cqrs;

namespace Sample.Commands
{
    public class ApproveAccount : ICommand
    {
        private ApproveAccount()
        {
        }

        public ApproveAccount(Guid accountId, string approvedBy) : this()
        {
            AccountId = accountId;
            ApprovedBy = approvedBy;
        }

        public Guid AccountId { get; private set; }
        public string ApprovedBy { get; private set; }
    }
}