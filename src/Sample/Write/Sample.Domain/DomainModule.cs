using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commands;
using Sample.Domain.CommandHandlers;
using Sample.Domain.DomainServices;
using Sample.Domain.EventHandlers;
using Sample.DomainEvents;
using Ximo.Cqrs;
using Ximo.Cqrs.Decorators;
using Ximo.DependencyInjection;
using Ximo.Domain;

namespace Sample.Domain
{
    public class DomainModule : IModule
    {
        public void Initialize(IServiceCollection builder)
        {
            RegisterDomainServices(builder);
            RegisterCommandHandlers(builder);
            RegisterEventHandlers(builder);
        }

        public IConfiguration Configuration { get; set; }

        private void RegisterCommandHandlers(IServiceCollection builder)
        {
            builder.RegisterCommandHandler<CreateAccount, CreateAccountHandler>();
            builder.Decorate<ICommandHandler<CreateAccount>, CommandLoggingDecorator<CreateAccount>>();

            builder.RegisterCommandHandler<UpdateAccountAddress, UpdateAccountAddressHandler>();
            builder.Decorate<ICommandHandler<UpdateAccountAddress>, CommandLoggingDecorator<UpdateAccountAddress>>();

            builder.RegisterCommandHandler<ApproveAccount, ApproveAccountHandler>();
            builder.Decorate<ICommandHandler<ApproveAccount>, CommandLoggingDecorator<ApproveAccount>>();

            builder.RegisterCommandHandler<DeleteAccount, DeleteAccountHandler>();
            builder.Decorate<ICommandHandler<DeleteAccount>, CommandLoggingDecorator<DeleteAccount>>();

            builder.RegisterCommandHandler<ReinstateAccount, ReinstateAccountHandler>();
            builder.Decorate<ICommandHandler<ReinstateAccount>, CommandLoggingDecorator<ReinstateAccount>>();
        }

        private void RegisterDomainServices(IServiceCollection builder)
        {
            builder.AddTransient<IAccountNumberGenerator, AccountNumberGenerator>();
        }

        private void RegisterEventHandlers(IServiceCollection builder)
        {
            builder.RegisterDomainEventHandler<AccountReinstated, AccountReinstatedHandler>();
        }

        public static void RegisterSubscriptions(IDomainEventBus domainEventBus)
        {
            domainEventBus.Subscribe<AccountReinstated, AccountReinstatedHandler>();
        }
    }
}