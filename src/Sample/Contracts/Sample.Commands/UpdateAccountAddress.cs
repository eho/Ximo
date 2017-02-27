using System;
using Ximo.Cqrs;

namespace Sample.Commands
{
    public class UpdateAccountAddress : ICommand
    {
        private UpdateAccountAddress()
        {
        }

        public UpdateAccountAddress(Guid accountId, string addressLine1, string addressLine2, string postcode,
            string city, string state, string countryName) : this()
        {
            AccountId = accountId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Postcode = postcode;
            City = city;
            State = state;
            CountryName = countryName;
        }

        public Guid AccountId { get; private set; }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string Postcode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string CountryName { get; private set; }
    }
}