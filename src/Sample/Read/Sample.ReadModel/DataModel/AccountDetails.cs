using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Sample.DomainEvents;
using Sample.Queries.Dtos;

namespace Sample.ReadModel.DataModel
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [Table("AccountDetails", Schema = "Read")]
    public class AccountDetails
    {
        private AccountDetails()
        {
        }

        public AccountDetails(AccountCreated @event) : this()
        {
            AccountId = @event.AccountId;
            AccountNumber = @event.AccountNumber;
            BusinessName = @event.BusinessName;
        }

        [Key]
        public Guid AccountId { get; private set; }

        public int AccountNumber { get; private set; }

        [Required]
        [MaxLength(100)]
        public string BusinessName { get; private set; }

        [MaxLength(100)]
        public string AddressLine1 { get; private set; }

        [MaxLength(100)]
        public string AddressLine2 { get; private set; }

        [MaxLength(100)]
        public string City { get; private set; }

        [MaxLength(12)]
        public string Postcode { get; private set; }

        [MaxLength(100)]
        public string State { get; private set; }

        [MaxLength(100)]
        public string CountryName { get; private set; }

        public bool IsApproved { get; private set; }

        [MaxLength(100)]
        public string ApprovedBy { get; private set; }

        public AccountDetailsDto ToAccountDetailsDto()
        {
            return new AccountDetailsDto(AccountId, AccountNumber, BusinessName, AddressLine1, AddressLine2, City,
                Postcode, State, CountryName);
        }

        public void SetAddressDetails(AddressUpdated @event)
        {
            AddressLine1 = @event.AddressLine1;
            AddressLine2 = @event.AddressLine2;
            City = @event.City;
            Postcode = @event.Postcode;
            State = @event.State;
            CountryName = @event.CountryName;
        }

        public void SetApproved(AccountApproved @event)
        {
            IsApproved = true;
            ApprovedBy = @event.ApprovedBy;
        }
    }
}