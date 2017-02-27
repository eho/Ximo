using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Sample.DomainEvents;
using Sample.Queries.Dtos;

namespace Sample.ReadModel.DataModel
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [Table("SystemTag", Schema = "Read")]
    public class SystemTag
    {
        private SystemTag()
        {
        }

        public SystemTag(SystemTagAdded @event) : this()
        {
            Name = @event.Name;
            AppliesToExpenses = @event.AppliesToExpenses;
            AppliesToTimesheets = @event.AppliesToTimesheets;
            AccountId = @event.AccountId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemTagId { get; private set; }

        public Guid AccountId { get; private set; }
        public string Name { get; private set; }
        public bool AppliesToExpenses { get; private set; }
        public bool AppliesToTimesheets { get; private set; }

        public SystemTagDto ToSystemTagDto()
        {
            return new SystemTagDto(AccountId, Name, AppliesToExpenses, AppliesToTimesheets);
        }
    }
}