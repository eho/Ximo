using System;

namespace Sample.Queries.Dtos
{
    public class AccountDetailsDto
    {
        protected AccountDetailsDto()
        {
        }

        public AccountDetailsDto(Guid accountId, int accountNumber, string businessName) : this()
        {
            AccountId = accountId;
            AccountNumber = accountNumber;
            BusinessName = businessName;
        }

        public AccountDetailsDto(Guid accountId, int accountNumber, string businessName, string addressLine1,
            string addressLine2, string city, string postcode, string state, string countryName)
            : this(accountId, accountNumber, businessName)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            Postcode = postcode;
            State = state;
            CountryName = countryName;
        }


        public Guid AccountId { get; protected set; }
        public int AccountNumber { get; protected set; }
        public string BusinessName { get; protected set; }
        public string AddressLine1 { get; protected set; }
        public string AddressLine2 { get; protected set; }
        public string City { get; protected set; }
        public string Postcode { get; protected set; }
        public string State { get; protected set; }
        public string CountryName { get; protected set; }
    }
}