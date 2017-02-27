using Ximo.Domain;
using Ximo.Validation;

namespace Sample.Domain.ValueObjects
{
    public class Address : ValueObject<Address>
    {
        private string _addressLine1;
        private string _addressLine2;
        private string _city;
        private string _countryName;
        private string _planet;
        private string _postcode;
        private string _state;

        private Address()
        {
        }

        public Address(string addressLine1 = null, string addressLine2 = null, string city = null,
            string postCode = null, string state = null, string countryName = null, string planet = null)
            : this()
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            Postcode = postCode;
            State = state;
            CountryName = countryName;
            Planet = planet;
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _addressLine1 = value;
            }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _addressLine2 = value;
            }
        }

        public string City
        {
            get { return _city; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _city = value;
            }
        }

        public string Planet
        {
            get { return _planet; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _planet = value;
            }
        }

        public string Postcode
        {
            get { return _postcode; }
            private set
            {
                FieldCheck.MaxLength(value, 12);
                _postcode = value;
            }
        }

        public string State
        {
            get { return _state; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _state = value;
            }
        }

        public string CountryName
        {
            get { return _countryName; }
            private set
            {
                FieldCheck.MaxLength(value, 100);
                _countryName = value;
            }
        }
    }
}