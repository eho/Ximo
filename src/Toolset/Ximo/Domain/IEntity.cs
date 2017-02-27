using System;

namespace Ximo.Domain
{
    /// <summary>
    ///     Contract for entities.
    /// </summary>
    /// <typeparam name="TId">The type of the primary identifier.</typeparam>
    public interface IEntity<TId> : IEquatable<IEntity<TId>>
        where TId : struct
    {
        /// <summary>
        ///     Gets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        TId Id { get; }
    }
}