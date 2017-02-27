using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ximo.EventSourcing;
using Ximo.Validation;

namespace Ximo.Ef.EventSourcing
{
    /// <summary>
    ///     An abstract base class for domain events using Entity Framework data context.
    /// </summary>
    public abstract class EfDomainEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EfDomainEvent" /> class.
        /// </summary>
        protected EfDomainEvent()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfDomainEvent" /> class.
        /// </summary>
        /// <param name="eventWrapper">The domain eventWrapper.</param>
        protected EfDomainEvent(DomainEventEnvelope eventWrapper)
        {
            Check.NotNull(eventWrapper, nameof(eventWrapper));

            var jsonObjectSerializer = new JsonObjectSerializer();

            EventId = eventWrapper.EventId;
            Name = eventWrapper.EventName;
            AggregateId = eventWrapper.AggregateId;
            Sequence = eventWrapper.Sequence;
            CreatedOnUtc = eventWrapper.CreatedOnUtc;
            Payload = jsonObjectSerializer.Serialize(eventWrapper.Event);
            AggregateVersion = eventWrapper.AggregateVersion;
        }

        /// <summary>
        ///     Gets or sets the unique identifier of the eventWrapper.
        /// </summary>
        [Key]
        public Guid EventId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier of the aggregate.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        ///     Gets or sets the payload.
        /// </summary>
        [Required]
        [MaxLength(4000)]
        public string Payload { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        [Column("EventSequence")]
        public int Sequence { get; set; }

        /// <summary>
        ///     Gets or sets the date and time when this instance was created (UTC).
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///     Gets the aggregate version.
        /// </summary>
        public int AggregateVersion { get; set; }
    }
}