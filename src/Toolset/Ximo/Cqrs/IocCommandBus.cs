using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     Default implementation for the command bus using dependency injection for routing commands.
    /// </summary>
    /// <seealso cref="Ximo.Cqrs.ICommandBus" />
    internal class IocCommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IocCommandBus" /> class.
        /// </summary>
        public IocCommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Routes the specified command to the relevant command handler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to be routed.</param>
        public void Send<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            Check.NotNull(command, nameof(command));
            var commandHandler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            commandHandler.Handle(command);
        }

        /// <summary>
        ///     Routes the specified command to the relevant command handler and executes asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to be routed.</param>
        /// <returns>Task.</returns>
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            await Task.Factory.StartNew(() => Send(command));
        }
    }
}