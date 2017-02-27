using System.Collections.Generic;
using System.Linq;

namespace Ximo.Domain.Tracking
{
    internal class ValueObjectComparer<TValueObject> where TValueObject : ValueObject<TValueObject>
    {
        private readonly TValueObject _newValue;
        private readonly TValueObject _oldValue;
        private readonly string _propertyName;

        public ValueObjectComparer(string propertyName, TValueObject oldValue, TValueObject newValue)
        {
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public IEnumerable<PropertyChange> GetChanges()
        {
            Dictionary<string, object> newValues;
            Dictionary<string, object> oldValues;
            if (_oldValue == null)
            {
                newValues = _newValue.GetValues();
                oldValues = newValues.Keys.ToDictionary<string, string, object>(key => key, key => null);
            }
            else if (_newValue == null)
            {
                oldValues = _oldValue.GetValues();
                newValues = oldValues.Keys.ToDictionary<string, string, object>(key => key, key => null);
            }
            else
            {
                oldValues = _oldValue.GetValues();
                newValues = _newValue.GetValues();
            }

            var list = new List<PropertyChange>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var key in oldValues.Keys)
            {
                if (!Equals(oldValues[key], newValues[key]))
                {
                    var propertyName = _propertyName + "." + key;
                    list.Add(new PropertyChange(propertyName, newValues[key], oldValues[key]));
                }
            }
            return list;
        }
    }
}