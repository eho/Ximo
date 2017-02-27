using Sample.Commands;
using Sample.Domain.Repositories;
using Ximo.Cqrs;

namespace Sample.Domain.CommandHandlers
{
    internal class DeleteAccountHandler : ICommandHandler<DeleteAccount>
    {
        private readonly IAccountStore _accountStore;

        public DeleteAccountHandler(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        public void Handle(DeleteAccount command)
        {
            var account = _accountStore.GetById(command.AccountId);
            account.Delete(command.Reason);
            _accountStore.Save(account);
        }
    }
}