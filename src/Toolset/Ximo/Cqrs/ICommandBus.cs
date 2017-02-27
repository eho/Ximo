using System.Threading.Tasks;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     The contract defined the router of commands to command handlers
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        ///     Routes the specified command to the relevant command handler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to be routed.</param>
        void Send<TCommand>(TCommand command) where TCommand : class, ICommand;

        /// <summary>
        ///     Routes the specified command to the relevant command handler and executes asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to be routed.</param>
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}