using Sample.Commands;
using Sample.Domain.DomainServices;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using Ximo.Cqrs;

namespace Sample.Domain.CommandHandlers
{
    internal class CreateAccountHandler : ICommandHandler<CreateAccount>
    {
        private readonly IAccountNumberGenerator _accountNumberGenerator;
        private readonly IAccountStore _accountStore;

        public CreateAccountHandler(IAccountNumberGenerator accountNumberGenerator, IAccountStore accountStore)
        {
            _accountNumberGenerator = accountNumberGenerator;
            _accountStore = accountStore;
        }

        public void Handle(CreateAccount command)
        {
            var newAccountNumber = _accountNumberGenerator.GenerateAccountNumber();
            var account = new Account(command.BusinessName, newAccountNumber, command.NewAccountId);
            _accountStore.Save(account);
        }
    }
}