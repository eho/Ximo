using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Cqrs;
using Ximo.Domain;

namespace Ximo.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadModule<TModule>(this IServiceCollection serviceCollection,
            IConfiguration configuration = null)
            where TModule : IModule, new()
        {
            var module = new TModule();
            if (configuration != null)
            {
                module.Configuration = configuration;
            }
            module.Initialize(serviceCollection);
            return serviceCollection;
        }

        public static IServiceCollection RegisterDefaultCommandBus(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<ICommandBus, IocCommandBus>();
        }

        public static IServiceCollection RegisterDefaultQueryProcessor(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IQueryProcessor, IocQueryProcessor>();
        }

        public static IServiceCollection RegisterDefaulSystemEventBus(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<ISystemEventBus, IocSystemEventBus>();
        }

        public static IServiceCollection RegisterDefaulDomainEventBus(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IDomainEventBus, IocDomainEventBus>();
        }

        public static IServiceCollection RegisterCommandHandler<TCommand, TCommandHandler>(
            this IServiceCollection serviceCollection)
            where TCommand : ICommand
            where TCommandHandler : class, ICommandHandler<TCommand>
        {
            return serviceCollection.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        }

        public static IServiceCollection RegisterQueryHandler<TQuery, TResult, TQueryHandler>(
            this IServiceCollection serviceCollection)
            where TQuery : class, IQuery<TResult>
            where TQueryHandler : class, IQueryHandler<TQuery, TResult>
        {
            return serviceCollection.AddTransient<IQueryHandler<TQuery, TResult>, TQueryHandler>();
        }

        public static IServiceCollection RegisterSystemEventHandler<TSystemEvent, TSystemEventHandler>(
            this IServiceCollection serviceCollection)
            where TSystemEvent : ISystemEvent
            where TSystemEventHandler : class, ISystemEventHandler<TSystemEvent>
        {
            return serviceCollection.AddTransient<ISystemEventHandler<TSystemEvent>, TSystemEventHandler>();
        }

        public static IServiceCollection RegisterDomainEventHandler<TDomainEvent, TDomainEventHandler>(
            this IServiceCollection serviceCollection)
            where TDomainEvent : IDomainEvent
            where TDomainEventHandler : class, IDomainEventHandler<TDomainEvent>
        {
            return serviceCollection.AddTransient<TDomainEventHandler>();
        }
    }
}