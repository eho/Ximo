using Sample.Domain.Data.DataModel;
using Sample.Domain.Repositories;
using Ximo.Ef;

namespace Sample.Domain.Data.Repositories
{
    internal class AccountNumberIndexStore : EfRepository<DomainDataContext>, IAccountNumberIndexStore
    {
        public AccountNumberIndexStore(DomainDataContext context) : base(context)
        {
        }

        public int GenerateNewAccountNumber()
        {
            var newRecord = Context.AccountNumberRegistryRecords.Add(new AccountNumberRegistryRecord());
            Context.SaveChanges();
            return newRecord.Entity.AccountNumberId;
        }
    }
}