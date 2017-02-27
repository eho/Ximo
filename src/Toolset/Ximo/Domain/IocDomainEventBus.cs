using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.Domain
{
    /// <summary>
    ///     Default implementation of the domain event bus using dependency injection to route events to handlers.
    /// </summary>
    /// <seealso cref="IDomainEventBus" />
    internal class IocDomainEventBus : IDomainEventBus
    {
        private static readonly ConcurrentDictionary<Type, List<Type>> Subscriptions =
            new ConcurrentDictionary<Type, List<Type>>();

        private readonly IServiceProvider _serviceProvider;

        public IocDomainEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Subscribe<TDomainEvent, TConcreteHandler>() where TDomainEvent : class, IDomainEvent
            where TConcreteHandler : IDomainEventHandler<TDomainEvent>
        {
            var concreteHandlerType = typeof(TConcreteHandler);
            var domainEventType = typeof(TDomainEvent);

            if (!Subscriptions.ContainsKey(domainEventType))
            {
                var eventSubscriptions = new List<Type> {concreteHandlerType};
                Subscriptions[domainEventType] = eventSubscriptions;
            }
            else
            {
                if (Subscriptions[domainEventType].Contains(concreteHandlerType))
                {
                    return;
                }

                var eventSubscriptions = Subscriptions[domainEventType];
                eventSubscriptions.Add(concreteHandlerType);
                Subscriptions[domainEventType] = eventSubscriptions;
            }
        }

        public void UnSubscribe<TDomainEvent, TConcreteHandler>() where TDomainEvent : class, IDomainEvent
            where TConcreteHandler : IDomainEventHandler<TDomainEvent>
        {
            var concreteHandlerType = typeof(TConcreteHandler);
            var domainEventType = typeof(TDomainEvent);

            if (!Subscriptions.ContainsKey(domainEventType) ||
                !Subscriptions[domainEventType].Contains(concreteHandlerType))
            {
                return;
            }

            var eventSubscriptions = Subscriptions[domainEventType];
            eventSubscriptions.Remove(concreteHandlerType);
            if (eventSubscriptions.Any())
            {
                Subscriptions[domainEventType] = eventSubscriptions;
            }
            else
            {
                Subscriptions.TryRemove(domainEventType, out eventSubscriptions);
            }
        }

        public void Publish<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class, IDomainEvent
        {
            Check.NotNull(@event, nameof(@event));

            var domainEventType = @event.GetType();

            if (!Subscriptions.ContainsKey(domainEventType))
            {
                if (throwWhenNotSubscribedTo)
                {
                    throw new InvalidOperationException(
                        $"No handler is registered for domain event '{domainEventType.FullName}'");
                }
                return;
            }

            var eventSubscriptions = Subscriptions[domainEventType];
            foreach (var eventHandler in eventSubscriptions)
            {
                var handler = _serviceProvider.GetRequiredService(eventHandler);
                var handle =
                    eventHandler.GetMethods()
                        .First(m => m.GetParameters()[0].ParameterType == domainEventType && m.Name.Equals("Handle"));
                handle.Invoke(handler, new object[] {@event});
            }
        }

        public async Task PublishAsync<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class, IDomainEvent
        {
            await Task.Factory.StartNew(() => { Publish(@event, throwWhenNotSubscribedTo); });
        }
    }
}