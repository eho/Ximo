using Ximo.Domain;

namespace Sample.Domain.Repositories
{
    public interface IAccountNumberIndexStore : IRepository
    {
        int GenerateNewAccountNumber();
    }
}