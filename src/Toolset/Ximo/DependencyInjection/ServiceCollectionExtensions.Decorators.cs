﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.DependencyInjection
{
    /// <summary>
    ///     https://github.com/khellang/Scrutor
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Decorates all registered services of type <typeparamref name="TService" />
        ///     using the specified type <typeparamref name="TDecorator" />.
        /// </summary>
        /// <param name="services">The services to add to.</param>
        /// <exception cref="InvalidOperationException">
        ///     If no service of the type <typeparamref name="TService" /> has been
        ///     registered.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="services" /> argument is <c>null</c>.</exception>
        public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services)
            where TDecorator : TService
        {
            return services.DecorateDescriptors(typeof(TService), x => x.Decorate(typeof(TDecorator)));
        }

        /// <summary>
        ///     Decorates all registered services of the specified <paramref name="serviceType" />
        ///     using the specified <paramref name="decoratorType" />.
        /// </summary>
        /// <param name="services">The services to add to.</param>
        /// <param name="serviceType">The type of services to decorate.</param>
        /// <param name="decoratorType">The type to decorate existing services with.</param>
        /// <exception cref="InvalidOperationException">
        ///     If no service of the specified <paramref name="serviceType" /> has been
        ///     registered.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     If either the <paramref name="services" />,
        ///     <paramref name="serviceType" /> or <paramref name="decoratorType" /> arguments are <c>null</c>.
        /// </exception>
        public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType, Type decoratorType)
        {
            Check.NotNull(decoratorType, nameof(decoratorType));

            return services.DecorateDescriptors(serviceType, x => x.Decorate(decoratorType));
        }

        /// <summary>
        ///     Decorates all registered services of type <typeparamref name="TService" />
        ///     using the <paramref name="decorator" /> function.
        /// </summary>
        /// <typeparam name="TService">The type of services to decorate.</typeparam>
        /// <param name="services">The services to add to.</param>
        /// <param name="decorator">The decorator function.</param>
        /// <exception cref="InvalidOperationException">If no service of <typeparamref name="TService" /> has been registered.</exception>
        /// <exception cref="ArgumentNullException">
        ///     If either the <paramref name="services" />
        ///     or <paramref name="decorator" /> arguments are <c>null</c>.
        /// </exception>
        public static IServiceCollection Decorate<TService>(this IServiceCollection services,
            Func<TService, IServiceProvider, TService> decorator)
        {
            Check.NotNull(decorator, nameof(decorator));

            return services.DecorateDescriptors(typeof(TService), x => x.Decorate(decorator));
        }

        /// <summary>
        ///     Decorates all registered services of type <typeparamref name="TService" />
        ///     using the <paramref name="decorator" /> function.
        /// </summary>
        /// <typeparam name="TService">The type of services to decorate.</typeparam>
        /// <param name="services">The services to add to.</param>
        /// <param name="decorator">The decorator function.</param>
        /// <exception cref="InvalidOperationException">If no service of <typeparamref name="TService" /> has been registered.</exception>
        /// <exception cref="ArgumentNullException">
        ///     If either the <paramref name="services" />
        ///     or <paramref name="decorator" /> arguments are <c>null</c>.
        /// </exception>
        public static IServiceCollection Decorate<TService>(this IServiceCollection services,
            Func<TService, TService> decorator)
        {
            Check.NotNull(decorator, nameof(decorator));

            return services.DecorateDescriptors(typeof(TService), x => x.Decorate<TService>(inner => decorator(inner)));
        }

        /// <summary>
        ///     Decorates all registered services of the specified <paramref name="serviceType" />
        ///     using the <paramref name="decorator" /> function.
        /// </summary>
        /// <param name="services">The services to add to.</param>
        /// <param name="serviceType">The type of services to decorate.</param>
        /// <param name="decorator">The decorator function.</param>
        /// <exception cref="InvalidOperationException">
        ///     If no service of the specified <paramref name="serviceType" /> has been
        ///     registered.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     If either the <paramref name="services" />,
        ///     <paramref name="serviceType" /> or <paramref name="decorator" /> arguments are <c>null</c>.
        /// </exception>
        public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType,
            Func<object, IServiceProvider, object> decorator)
        {
            Check.NotNull(decorator, nameof(decorator));

            return services.DecorateDescriptors(serviceType, x => x.Decorate(decorator));
        }

        /// <summary>
        ///     Decorates all registered services of the specified <paramref name="serviceType" />
        ///     using the <paramref name="decorator" /> function.
        /// </summary>
        /// <param name="services">The services to add to.</param>
        /// <param name="serviceType">The type of services to decorate.</param>
        /// <param name="decorator">The decorator function.</param>
        /// <exception cref="InvalidOperationException">
        ///     If no service of the specified <paramref name="serviceType" /> has been
        ///     registered.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     If either the <paramref name="services" />,
        ///     <paramref name="serviceType" /> or <paramref name="decorator" /> arguments are <c>null</c>.
        /// </exception>
        public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType,
            Func<object, object> decorator)
        {
            Check.NotNull(decorator, nameof(decorator));

            return services.DecorateDescriptors(serviceType, x => x.Decorate(decorator));
        }

        private static IServiceCollection DecorateDescriptors(this IServiceCollection services, Type serviceType,
            Func<ServiceDescriptor, ServiceDescriptor> decorator)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(serviceType, nameof(serviceType));

            var descriptors = services.GetDescriptors(serviceType);

            foreach (var descriptor in descriptors)
            {
                var index = services.IndexOf(descriptor);

                // To avoid reordering descriptors, in case a specific order is expected.
                services.Insert(index, decorator(descriptor));

                services.Remove(descriptor);
            }

            return services;
        }

        private static List<ServiceDescriptor> GetDescriptors(this IServiceCollection services, Type serviceType)
        {
            var descriptors = services.Where(service => service.ServiceType == serviceType).ToList();

            if (descriptors.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Could not find any registered services for type '{serviceType.FullName}'.");
            }

            return descriptors;
        }

        private static ServiceDescriptor Decorate<TService>(this ServiceDescriptor descriptor,
            Func<TService, IServiceProvider, TService> decorator)
        {
            return descriptor.WithFactory(provider => decorator((TService) provider.GetInstance(descriptor), provider));
        }

        private static ServiceDescriptor Decorate<TService>(this ServiceDescriptor descriptor,
            Func<TService, TService> decorator)
        {
            return descriptor.WithFactory(provider => decorator((TService) provider.GetInstance(descriptor)));
        }

        private static ServiceDescriptor Decorate(this ServiceDescriptor descriptor, Type decoratorType)
        {
            return
                descriptor.WithFactory(
                    provider => provider.CreateInstance(decoratorType, provider.GetInstance(descriptor)));
        }

        private static ServiceDescriptor WithFactory(this ServiceDescriptor descriptor,
            Func<IServiceProvider, object> factory)
        {
            return ServiceDescriptor.Describe(descriptor.ServiceType, factory, descriptor.Lifetime);
        }

        private static object GetInstance(this IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationType != null)
            {
                return provider.GetServiceOrCreateInstance(descriptor.ImplementationType);
            }

            return descriptor.ImplementationFactory(provider);
        }

        private static object GetServiceOrCreateInstance(this IServiceProvider provider, Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
        }

        private static object CreateInstance(this IServiceProvider provider, Type type, params object[] arguments)
        {
            return ActivatorUtilities.CreateInstance(provider, type, arguments);
        }
    }
}