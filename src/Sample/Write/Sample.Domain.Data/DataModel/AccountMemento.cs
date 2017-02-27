using System.ComponentModel.DataAnnotations.Schema;
using Sample.Domain.Entities;
using Ximo.Ef.EventSourcing;

namespace Sample.Domain.Data.DataModel
{
    [Table("AccountSnapshot", Schema = "Write")]
    public class AccountMemento : EfAggregateMemento
    {
        internal AccountMemento()
        {
        }

        public AccountMemento(Account account) : base(account)
        {
        }
    }
}