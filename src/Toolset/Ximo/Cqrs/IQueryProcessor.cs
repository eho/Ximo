using System.Threading.Tasks;

namespace Ximo.Cqrs
{
    /// <summary>
    ///     Defines the contract for a query router.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        ///     Processes the query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The relevant query response.</returns>
        TResult ProcessQuery<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;

        /// <summary>
        ///     Processes the query asynchronously.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The relevant query response.</returns>
        Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
    }
}