using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Ximo.Cqrs.Decorators
{
    public sealed class CommandLoggingDecorator<TCommand> : ICommandDecorator<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly ILogger _logger;

        public CommandLoggingDecorator(ICommandHandler<TCommand> decorated,
            ILogger<CommandLoggingDecorator<TCommand>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public void Handle(TCommand command)
        {
            var commandName = command.GetType().Name;

            _logger.LogInformation($"Start executing command '{commandName}'" + Environment.NewLine);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                _decorated.Handle(command);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception throw while executing command '{commandName}'" +
                                 Environment.NewLine + e.Message + Environment.NewLine);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.LogInformation($"Executed command '{commandName}' in {stopwatch.Elapsed}." +
                                   Environment.NewLine);
        }
    }
}