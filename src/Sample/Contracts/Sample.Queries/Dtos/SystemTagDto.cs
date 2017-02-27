using System;

namespace Sample.Queries.Dtos
{
    public class SystemTagDto
    {
        protected SystemTagDto()
        {
        }

        public SystemTagDto(Guid accountId, string name, bool appliesToExpenses, bool appliesToTimesheets) : this()
        {
            AccountId = accountId;
            Name = name;
            AppliesToExpenses = appliesToExpenses;
            AppliesToTimesheets = appliesToTimesheets;
        }

        public Guid AccountId { get; protected set; }
        public string Name { get; protected set; }
        public bool AppliesToExpenses { get; protected set; }
        public bool AppliesToTimesheets { get; protected set; }
    }
}