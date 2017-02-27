using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Domain.Data.DataModel
{
    [Table("AccountNumber", Schema = "Write")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class AccountNumberRegistryRecord
    {
        public AccountNumberRegistryRecord()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumberId { get; private set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOnUtc { get; private set; }
    }
}