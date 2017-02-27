using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Domain.Data.DataModel;
using Sample.Domain.Data.Repositories;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using Ximo.DependencyInjection;
using Ximo.Ef.EventSourcing;
using Ximo.EventSourcing;

namespace Sample.Domain.Data
{
    public class DomainDataModule : IModule
    {
        public void Initialize(IServiceCollection builder)
        {
            RegisterContext(builder);
            RegisterRepositories(builder);
        }

        public IConfiguration Configuration { private get; set; }

        private void RegisterContext(IServiceCollection builder)
        {
            var connectionString = Configuration.GetConnectionString("SampleDatabase");
            builder.AddDbContext<DomainDataContext>(options => options.UseSqlServer(connectionString).UseLoggerFactory(null));
        }

        private static void RegisterRepositories(IServiceCollection builder)
        {
            builder.AddTransient<IAccountNumberIndexStore, AccountNumberIndexStore>();
            builder.AddTransient<IAccountStore, AccountStore>();
            builder
                .AddTransient
                <ISnapshotRepository<Account>, EfSnapshotRepository<Account, AccountMemento, DomainDataContext>>();
        }
    }
}