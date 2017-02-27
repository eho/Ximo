using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Ximo.Domain.Tracking
{
    /// <summary>
    ///     Represents the base class for entities that need to track changes.
    /// </summary>
    /// <typeparam name="TId">The type of the entity identifier.</typeparam>
    /// <typeparam name="TVersionType">The type of the version property.</typeparam>
    public abstract class TrackedEntity<TId, TVersionType> : Entity<TId>, ITrackedEntity<TId, TVersionType>
        where TId : struct
    {
        protected TrackedEntity()
        {
            ChangeLog = new ChangeLog();
            CreatedOnUtc = DateTime.UtcNow;
            IsTracking = false;
        }

        /// <summary>
        ///     Gets or sets the change log.
        /// </summary>
        /// <value>The change log.</value>
        [NotMapped]
        public ChangeLog ChangeLog { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is tracking changes.
        /// </summary>
        /// <value><c>true</c> if this instance is tracking changes; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool IsTracking { get; protected set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool HasChanges => ChangeLog.Changes.Count > 0;

        /// <summary>
        ///     Gets or sets the created time in UTC.
        /// </summary>
        /// <value>The created time in UTC.</value>
        public DateTime CreatedOnUtc { get; protected internal set; }

        /// <summary>
        ///     Gets or sets the modified time in UTC.
        /// </summary>
        /// <value>The modified time in UTC.</value>
        public DateTime? ModifiedOnUtc { get; protected internal set; }

        /// <summary>
        ///     Starts change tracking on this entity.
        /// </summary>
        public void StartTracking()
        {
            IsTracking = true;
            OnStartTracking();
        }

        /// <summary>
        ///     Stops the change tracking.
        /// </summary>
        public void StopTracking()
        {
            IsTracking = false;
            OnStopTracking();
        }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public TVersionType Version { get; protected internal set; }

        /// <summary>
        ///     Sets the field values while tracking changes if change tracking is enabled.
        /// </summary>
        /// <typeparam name="TFieldType">The type of the field.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property is set to a new value, <c>false</c> otherwise.</returns>
        protected override bool SetField<TFieldType>(ref TFieldType field, TFieldType value,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<TFieldType>.Default.Equals(field, value))
            {
                return false;
            }
            if (IsTracking)
            {
                var propertyChange = new PropertyChange(propertyName, value, field);
                ChangeLog.Changes.Add(propertyChange);
            }
            if (typeof(TFieldType) == typeof(string) &&
                (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                field = default(TFieldType);
            }
            else
            {
                field = value;
            }
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
        protected override bool SetValueObjectField<TValueObject>(ref TValueObject field, TValueObject value,
            [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }
            if (IsTracking)
            {
                var comparer = new ValueObjectComparer<TValueObject>(propertyName, field, value);
                ChangeLog.Changes.AddRange(comparer.GetChanges());
            }
            field = value;
            return true;
        }

        /// <summary>
        ///     Called when change tracking is activated.
        /// </summary>
        protected virtual void OnStartTracking()
        {
        }

        /// <summary>
        ///     Called when change tracking is stopped.
        /// </summary>
        protected virtual void OnStopTracking()
        {
            if (HasChanges)
            {
                ModifiedOnUtc = DateTime.UtcNow;
            }
        }
    }
}