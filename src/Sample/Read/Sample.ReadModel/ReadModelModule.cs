using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.DomainEvents;
using Sample.Queries;
using Sample.Queries.Responses;
using Sample.ReadModel.EventHandlers;
using Sample.ReadModel.QueryHandlers;
using Ximo.Cqrs;
using Ximo.Cqrs.Decorators;
using Ximo.DependencyInjection;
using Ximo.Domain;

namespace Sample.ReadModel
{
    public class ReadModelModule : IModule
    {
        public void Initialize(IServiceCollection builder)
        {
            RegisterContext(builder);
            RegisterEventHandlers(builder);
            RegisterQueryHandlers(builder);
        }

        public IConfiguration Configuration { get; set; }

        private void RegisterContext(IServiceCollection builder)
        {
            builder.AddDbContext<ReadModelContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("SampleDatabase")).UseLoggerFactory(null));
        }

        private void RegisterEventHandlers(IServiceCollection builder)
        {
            builder.RegisterDomainEventHandler<AccountCreated, AccountCreatedHandler>();
            builder.RegisterDomainEventHandler<SystemTagAdded, SystemTagAddedHandler>();
            builder.RegisterDomainEventHandler<AddressUpdated, AddressUpdatedHandler>();
            builder.RegisterDomainEventHandler<AccountApproved, AccountApprovedHandler>();
            builder.RegisterDomainEventHandler<AccountDeleted, AccountDeletedHandler>();
        }

        private void RegisterQueryHandlers(IServiceCollection builder)
        {
            builder
                .RegisterQueryHandler
                <GetAccountDetailsById, GetAccountDetailsByIdResponse, GetFullAccountDetailsHandler>();

            builder
                .Decorate
                <IQueryHandler<GetAccountDetailsById, GetAccountDetailsByIdResponse>,
                    QueryLoggingDecorator<GetAccountDetailsById, GetAccountDetailsByIdResponse>>();
        }

        public static void RegisterSubscriptions(IDomainEventBus domainEventBus)
        {
            domainEventBus.Subscribe<AccountCreated, AccountCreatedHandler>();
            domainEventBus.Subscribe<SystemTagAdded, SystemTagAddedHandler>();
            domainEventBus.Subscribe<AddressUpdated, AddressUpdatedHandler>();
            domainEventBus.Subscribe<AccountApproved, AccountApprovedHandler>();
            domainEventBus.Subscribe<AccountDeleted, AccountDeletedHandler>();
        }
    }
}