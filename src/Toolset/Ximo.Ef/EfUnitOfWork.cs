using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Domain;

namespace Ximo.Ef
{
    /// <summary>
    ///     An implementation of <see cref="IUnitOfWork" /> using Entity Framework data context.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="DisposableObject" />
    public abstract class EfUnitOfWork<TContext> : DisposableObject, IUnitOfWork
        where TContext : DbContext
    {
        private readonly IServiceProvider _container;
        private readonly TContext _context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfUnitOfWork{TContext}" /> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="container">The dependency injection container.</param>
        protected EfUnitOfWork(TContext context, IServiceProvider container)
        {
            _context = context;
            _container = container;
        }

        /// <summary>
        ///     Gets the repositories of the supplied type.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <returns>The repository instance.</returns>
        public TRepository Repository<TRepository>()
            where TRepository : class, IRepository
        {
            return _container.GetRequiredService<TRepository>();
        }

        /// <summary>
        ///     Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                var rethrow = OnSaveErrorRethrow(exception);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                var rethrow = OnSaveErrorRethrow(exception);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     When overridden in implementing objects, returns whether a caught exception should be re-thrown.
        /// </summary>
        /// <param name="exception">The caught exception.</param>
        /// <returns>True if the exception should be re-thrown; otherwise false.</returns>
        protected virtual bool OnSaveErrorRethrow(Exception exception)
        {
            return true;
        }

        /// <summary>
        ///     When overridden in implementing objects, performs actual clean-up.
        /// </summary>
        protected override void Disposing()
        {
            _context?.Dispose();
        }
    }
}