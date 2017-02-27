using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Validation;

namespace Ximo.Cqrs
{
    internal class IocQueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public IocQueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult ProcessQuery<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
        {
            Check.NotNull(query, nameof(query));
            return _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>().Read(query);
        }

        public async Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            return await Task<TResult>.Factory.StartNew(() => ProcessQuery<TQuery, TResult>(query));
        }
    }
}