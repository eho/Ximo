using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Ximo.Extensions;
using Ximo.Utilities;

namespace Ximo.Domain
{
    /// <summary>
    ///     Represents a value object as specified by Domain Driven Design rules. The object has no identifier and equality is
    ///     achieved by comparing property values.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the t value object.</typeparam>
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        private IEnumerable<PropertyInfo> _properties;
        private Type _valueObjectType;

        private Type ValueObjectType => _valueObjectType ?? (_valueObjectType = GetType());

        private IEnumerable<PropertyInfo> Properties
        {
            get
            {
                if (_properties != null)
                {
                    return _properties;
                }
                return _properties = ReferenceObjectPropertyCache.GetProperties(ValueObjectType);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool HasValue
        {
            get { return Properties.Any(property => property.GetValue(this) != property.PropertyType.DefaultValue()); }
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(TValueObject other)
        {
            if (other == null)
            {
                return false;
            }

            if (ValueObjectType != other.GetType())
            {
                return false;
            }

            foreach (var property in Properties)
            {
                var value1 = property.GetValue(other);
                var value2 = property.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                    {
                        return false;
                    }
                }
                else if (!value1.Equals(value2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var item = obj as ValueObject<TValueObject>;

            if (item != null)
            {
                return Equals((TValueObject) item);
            }
            return false;
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            const int startValue = 17;
            const int multiplier = 59;
            return Properties.Select(propertyInfo => propertyInfo.GetValue(this))
                .Where(value => value != null)
                .Aggregate(startValue, (current, value) => current * multiplier + value.GetHashCode());
        }

        public static bool operator ==(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if ((object) x == null || (object) y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            return !(x == y);
        }

        internal Dictionary<string, object> GetValues()
        {
            return Properties.ToDictionary(property => property.Name, property => property.GetValue(this));
        }
    }
}