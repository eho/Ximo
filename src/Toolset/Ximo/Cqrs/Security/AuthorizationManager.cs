using System;
using System.Linq;
using System.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Ximo.Cqrs.Security
{
    /// <summary>
    ///     Default implementation of the authorization manager. This class cannot be inherited. It uses the defined
    ///     authorization rules.
    /// </summary>
    /// <seealso cref="Ximo.Cqrs.Security.IAuthorizationManager" />
    public sealed class AuthorizationManager : IAuthorizationManager
    {
        private readonly IServiceProvider _dependencyContainer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationManager" /> class.
        /// </summary>
        /// <param name="dependencyContainer">The dependency container.</param>
        public AuthorizationManager(IServiceProvider dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        /// <summary>
        ///     Authorizes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <exception cref="SecurityException"></exception>
        public void Authorize<TMessage>(TMessage message) where TMessage : IMessage
        {
            var messageRules = _dependencyContainer.GetRequiredService<IMessageAuthorisationRules<TMessage>>();

            var securityExceptions =
            (from rule in messageRules.Rules
                where !rule.IsAuthorized(message)
                select new SecurityException(rule.ErrorText)).ToList();

            switch (securityExceptions.Count)
            {
                case 0:
                    return;
                case 1:
                    throw securityExceptions.First();
                default:
                    var securityAggregateException = new AggregateException(securityExceptions);
                    throw new SecurityException(
                        "Multiple authorisation rules broken. Please see inner exception for details.",
                        securityAggregateException);
            }
        }
    }
}