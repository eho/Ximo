using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Ximo.Domain
{
    /// <summary>
    ///     Default Base Entity.
    /// </summary>
    /// <typeparam name="TId">The type of the primary identifier.</typeparam>
    public abstract class Entity<TId> : IEntity<TId>
        where TId : struct
    {
        /// <summary>
        ///     Gets or sets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        [NotMapped]
        public TId Id { get; protected set; }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(IEntity<TId> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || other.Id.Equals(Id);
        }

        /// <summary>
        ///     Sets the field values.
        /// </summary>
        /// <typeparam name="TFieldType">The type of the field.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property is set to a new value, <c>false</c> otherwise.</returns>
        protected virtual bool SetField<TFieldType>(
            ref TFieldType field,
            TFieldType value,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<TFieldType>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            return true;
        }

        /// <summary>
        ///     Sets a value object field while tracking changes if change tracking is enabled.
        /// </summary>
        /// <typeparam name="TValueObject">The type of the value object.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property is set to a new value, <c>false</c> otherwise.</returns>
        protected virtual bool SetValueObjectField<TValueObject>(
            ref TValueObject field,
            TValueObject value,
            [CallerMemberName] string propertyName = "")
            where TValueObject : ValueObject<TValueObject>
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            return true;
        }
    }
}