using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     Default implementation of the system event bus using dependency injection to route events to handlers.
    /// </summary>
    /// <seealso cref="ISystemEventBus" />
    internal class IocSystemEventBus : ISystemEventBus
    {
        private static readonly ConcurrentDictionary<Type, List<Type>> Subscriptions =
            new ConcurrentDictionary<Type, List<Type>>();

        private readonly IServiceProvider _serviceProvider;

        public IocSystemEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Subscribe<TSystemEvent, TConcreteHandler>() where TSystemEvent : class, ISystemEvent
            where TConcreteHandler : ISystemEventHandler<TSystemEvent>
        {
            var concreteHandlerType = typeof(TConcreteHandler);
            var systemEventType = typeof(TSystemEvent);

            if (!Subscriptions.ContainsKey(systemEventType))
            {
                var eventSubscriptions = new List<Type> {concreteHandlerType};
                Subscriptions[systemEventType] = eventSubscriptions;
            }
            else
            {
                if (Subscriptions[systemEventType].Contains(concreteHandlerType))
                {
                    return;
                }

                var eventSubscriptions = Subscriptions[systemEventType];
                eventSubscriptions.Add(concreteHandlerType);
                Subscriptions[systemEventType] = eventSubscriptions;
            }
        }

        public void UnSubscribe<TSystemEvent, TConcreteHandler>() where TSystemEvent : class, ISystemEvent
            where TConcreteHandler : ISystemEventHandler<TSystemEvent>
        {
            var concreteHandlerType = typeof(TConcreteHandler);
            var systemEventType = typeof(TSystemEvent);

            if (!Subscriptions.ContainsKey(systemEventType) ||
                !Subscriptions[systemEventType].Contains(concreteHandlerType))
            {
                return;
            }

            var eventSubscriptions = Subscriptions[systemEventType];
            eventSubscriptions.Remove(concreteHandlerType);
            if (eventSubscriptions.Any())
            {
                Subscriptions[systemEventType] = eventSubscriptions;
            }
            else
            {
                Subscriptions.TryRemove(systemEventType, out eventSubscriptions);
            }
        }

        public void Publish<TSystemEvent>(TSystemEvent @event, bool throwWhenNotSubscribedTo = true)
            where TSystemEvent : class, ISystemEvent
        {
            Check.NotNull(@event, nameof(@event));

            var systemEventType = @event.GetType();

            if (!Subscriptions.ContainsKey(systemEventType))
            {
                if (throwWhenNotSubscribedTo)
                {
                    throw new InvalidOperationException(
                        $"No handler is registered for system event '{systemEventType.FullName}'");
                }
                return;
            }

            var eventSubscriptions = Subscriptions[systemEventType];
            foreach (var eventHandler in eventSubscriptions)
            {
                var handler = _serviceProvider.GetRequiredService(eventHandler);
                var handle =
                    eventHandler.GetTypeInfo().GetMethods()
                        .First(m => m.GetParameters()[0].ParameterType == systemEventType && m.Name.Equals("Handle"));
                handle.Invoke(handler, new object[] {@event});
            }
        }

        public async Task PublishAsync<TSystemEvent>(TSystemEvent @event, bool throwWhenNotSubscribedTo = true)
            where TSystemEvent : class, ISystemEvent
        {
            await Task.Factory.StartNew(() => { Publish(@event, throwWhenNotSubscribedTo); });
        }
    }
}