using Ximo.Validation;

namespace Ximo.Domain.Tracking
{
    /// <summary>
    ///     Class representing the structure of a property value change.
    /// </summary>
    public sealed class PropertyChange
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyChange" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        public PropertyChange(string propertyName, object newValue, object oldValue = null)
        {
            Check.NotNull(propertyName, nameof(propertyName));
            PropertyName = propertyName;
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        ///     Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        ///     Gets the old value.
        /// </summary>
        /// <value>The old value.</value>
        public object OldValue { get; private set; }

        /// <summary>
        ///     Gets the new value.
        /// </summary>
        /// <value>The new value.</value>
        public object NewValue { get; private set; }
    }
}