using System;

namespace Ximo.Domain.Tracking
{
    public interface ITrackedEntity<TId, out TVersionType> : IEntity<TId> where TId : struct
    {
        DateTime CreatedOnUtc { get; }
        DateTime? ModifiedOnUtc { get; }
        ChangeLog ChangeLog { get; }
        bool IsTracking { get; }
        bool HasChanges { get; }
        TVersionType Version { get; }
        void StartTracking();
        void StopTracking();
    }
}