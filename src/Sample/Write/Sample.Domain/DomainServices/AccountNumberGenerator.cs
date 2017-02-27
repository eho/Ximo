using Sample.Domain.Repositories;

namespace Sample.Domain.DomainServices
{
    internal class AccountNumberGenerator : IAccountNumberGenerator
    {
        private readonly IAccountNumberIndexStore _accountNumberIndexStore;

        public AccountNumberGenerator(IAccountNumberIndexStore accountNumberIndexStore)
        {
            _accountNumberIndexStore = accountNumberIndexStore;
        }

        public int GenerateAccountNumber()
        {
            return _accountNumberIndexStore.GenerateNewAccountNumber();
        }
    }
}