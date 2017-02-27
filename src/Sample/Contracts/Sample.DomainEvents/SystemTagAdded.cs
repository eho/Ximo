using System;
using Ximo.Domain;

namespace Sample.DomainEvents
{
    public class SystemTagAdded : IDomainEvent
    {
        private SystemTagAdded()
        {
        }

        public SystemTagAdded(Guid accountId, string name, bool appliesToExpenses, bool appliesToTimesheets) : this()
        {
            Name = name;
            AppliesToExpenses = appliesToExpenses;
            AppliesToTimesheets = appliesToTimesheets;
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
        public string Name { get; private set; }
        public bool AppliesToExpenses { get; private set; }
        public bool AppliesToTimesheets { get; private set; }
    }
}