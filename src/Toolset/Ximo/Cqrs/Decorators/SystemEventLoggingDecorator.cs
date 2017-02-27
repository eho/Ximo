using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Ximo.Cqrs.Decorators
{
    public sealed class SystemEventLoggingDecorator<TSystemEvent> : ISystemEventDecorator<TSystemEvent>
        where TSystemEvent : class, ISystemEvent
    {
        private readonly ISystemEventHandler<TSystemEvent> _decorated;
        private readonly ILogger _logger;

        public SystemEventLoggingDecorator(ISystemEventHandler<TSystemEvent> decorated,
            ILogger<SystemEventLoggingDecorator<TSystemEvent>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public void Handle(TSystemEvent @event)
        {
            var systemEventName = @event.GetType().Name;

            _logger.LogInformation($"Start executing system event '{systemEventName}'" + Environment.NewLine);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                _decorated.Handle(@event);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception throw while executing system event '{systemEventName}'" +
                                 Environment.NewLine + e.Message + Environment.NewLine);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.LogInformation($"Executed system event '{systemEventName}' in {stopwatch.Elapsed}." +
                                   Environment.NewLine);
        }
    }
}