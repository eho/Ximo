using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ximo.EventSourcing;
using Ximo.Utilities;

namespace Ximo.Ef.EventSourcing
{
    /// <summary>
    ///     An implementation of <see cref="IAggregateMemento" /> using Entity Framework.
    /// </summary>
    public abstract class EfAggregateMemento : IAggregateMemento
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EfAggregateMemento" /> class.
        /// </summary>
        protected EfAggregateMemento()
        {
            SnapshotId = GuidFactory.NewGuidComb();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfAggregateMemento" /> class.
        /// </summary>
        /// <param name="aggregate">The aggregate instance.</param>
        protected EfAggregateMemento(EventSourcedAggregateRoot aggregate):this()
        {
            if (aggregate == null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            AggregateRootId = aggregate.Id;
            LastEventSequence = aggregate.LastEventSequence;
            Version = aggregate.Version;
            Payload = JsonObjectSerializer.New().Serialize(aggregate);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SnapshotId { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier of the aggregate.
        /// </summary>
        public Guid AggregateRootId { get; set; }

        /// <summary>
        ///     Gets or sets the last event sequence.
        /// </summary>
        public int LastEventSequence { get; set; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        public int Version { get; }

        /// <summary>
        ///     Gets or sets the payload.
        /// </summary>
        // [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string Payload { get; set; }
    }
}