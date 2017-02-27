using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Ximo.Cqrs.Decorators
{
    public sealed class QueryLoggingDecorator<TQuery, TResult> : IQueryDecorator<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;
        private readonly ILogger<QueryLoggingDecorator<TQuery, TResult>> _logger;

        public QueryLoggingDecorator(IQueryHandler<TQuery, TResult> decorated,
            ILogger<QueryLoggingDecorator<TQuery, TResult>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public TResult Read(TQuery query)
        {
            TResult result;
            var queryName = query.GetType().Name;

            _logger.LogInformation($"Start reading query '{queryName}'" + Environment.NewLine);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                result = _decorated.Read(query);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception throw while reading query '{queryName}'" +
                                 Environment.NewLine + e.Message + Environment.NewLine);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.LogInformation($"Executed query '{queryName}' in {stopwatch.Elapsed}." +
                                   Environment.NewLine);

            return result;
        }
    }
}