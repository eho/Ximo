using System.Collections.Generic;
using Ximo.Extensions;

namespace Ximo.Domain.Tracking
{
    /// <summary>
    ///     Class represents the Change log on an object maintaining the list of <see cref="PropertyChange" />. The class is
    ///     usually attached to an aggregate root.
    /// </summary>
    public class ChangeLog
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeLog" /> class.
        /// </summary>
        public ChangeLog()
        {
            Changes = new List<PropertyChange>();
        }

        /// <summary>
        ///     Gets the property changes.
        /// </summary>
        /// <value>The property changes.</value>
        public List<PropertyChange> Changes { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        public bool HasChanges => Changes.IsNotNullOrEmpty();
    }
}