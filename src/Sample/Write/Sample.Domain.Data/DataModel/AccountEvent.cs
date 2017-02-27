using System.ComponentModel.DataAnnotations.Schema;
using Ximo.Ef.EventSourcing;
using Ximo.EventSourcing;

namespace Sample.Domain.Data.DataModel
{
    [Table("AccountEvents", Schema = "Write")]
    public class AccountEvent : EfDomainEvent
    {
        // ReSharper disable once UnusedMember.Local
        private AccountEvent()
        {
        }

        public AccountEvent(DomainEventEnvelope eventWrapper) : base(eventWrapper)
        {
        }
    }
}