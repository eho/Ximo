using Ximo.Domain;
using Ximo.Validation;

namespace Sample.Domain.ValueObjects
{
    public class SystemTag : ValueObject<SystemTag>
    {
        private string _name;

        private SystemTag()
        {
        }

        public SystemTag(string name, bool appliesToExpenses, bool appliesToTimesheets) : this()
        {
            _name = name;
            AppliesToExpenses = appliesToExpenses;
            AppliesToTimesheets = appliesToTimesheets;
        }

        public bool AppliesToExpenses { get; }
        public bool AppliesToTimesheets { get; }

        public string Name
        {
            get { return _name; }
            // ReSharper disable once UnusedMember.Local
            private set
            {
                FieldCheck.NotNullOrWhitespace(value);
                _name = value;
            }
        }
    }
}