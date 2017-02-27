using System.Threading.Tasks;

namespace Ximo.Domain
{
    /// <summary>
    ///     Interface defining a unit of work implementation.
    /// </summary>
    /// <remarks>
    ///     see: http://martinfowler.com/eaaCatalog/unitOfWork.html
    /// </remarks>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Returns the repository representing the specified type.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <returns>The repository implementation of the specified type.</returns>
        TRepository Repository<TRepository>() where TRepository : class, IRepository;

        /// <summary>
        ///     Commits the changes that occurred within the scope of the unit of work.
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Commits the changes that occurred within the scope of the unit of work.
        /// </summary>
        Task SaveChangesAsync();
    }
}