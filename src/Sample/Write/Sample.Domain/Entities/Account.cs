using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sample.Domain.Exceptions;
using Sample.Domain.ValueObjects;
using Sample.DomainEvents;
using Ximo.EventSourcing;
using Ximo.Utilities;
using Ximo.Validation;

namespace Sample.Domain.Entities
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class Account : EventSourcedAggregateRoot
    {
        private int _accountNumber;
        private Address _address;
        private string _businessName;
        private AccountStatus _status;

        private Account()
        {
            Address = new Address();
            Status = new AccountStatus(false, null, false, null);
            SystemTags = new ReadOnlyCollection<SystemTag>(new List<SystemTag>());
        }

        public Account(string businessName, int accountNumber, Guid? newAccountId) : this()
        {
            var accountId = newAccountId ?? GuidFactory.NewGuidComb();
            ApplyChange(new AccountCreated(accountId, businessName, accountNumber));

            AddSystemTag("Transportation", true, false);
            AddSystemTag("Sick Leave", false, true);
            AddSystemTag("Training", true, true);
        }

        public int AccountNumber
        {
            get { return _accountNumber; }
            private set { SetField(ref _accountNumber, value); }
        }

        public ReadOnlyCollection<SystemTag> SystemTags { get; private set; }

        public string BusinessName
        {
            get { return _businessName; }
            private set
            {
                FieldCheck.NotNullOrWhitespace(value);
                FieldCheck.MaxLength(value, 100);
                SetField(ref _businessName, value);
            }
        }

        public Address Address
        {
            get { return _address; }
            private set { SetField(ref _address, value); }
        }

        public AccountStatus Status
        {
            get { return _status; }
            private set { SetValueObjectField(ref _status, value); }
        }

        public void AddSystemTag(string name, bool appliesToExpenses, bool appliesToTimesheets)
        {
            if (SystemTags.Any(x => x.Name.Equals(name)))
            {
                throw DuplicateSystemTagException.Create(name);
            }
            ApplyChange(new SystemTagAdded(Id, name, appliesToExpenses, appliesToTimesheets));
        }

        public void ChangeAddress(string addressLine1 = null, string addressLine2 = null, string city = null,
            string postCode = null, string state = null, string countryName = null)
        {
            ApplyChange(new AddressUpdated(Id, addressLine1, addressLine2, city, postCode, state, countryName));
        }

        public void Approve(string approvedBy)
        {
            //Contextual Validation
            Check.NotNullOrWhitespace(approvedBy, nameof(approvedBy));
            if (Status.IsApproved)
            {
                throw AccountApprovedException.Create();
            }
            ApplyChange(new AccountApproved(Id, approvedBy));
        }

        public void Delete(string reason)
        {
            //Contextual Validation
            Check.NotNullOrWhitespace(reason, nameof(reason));
            ApplyChange(new AccountDeleted(Id, reason));
        }

        public void Reinstate()
        {
            if (!Status.IsDeleted)
            {
                throw AccountIsNotDeletedException.Create();
            }
            ApplyChange(new AccountReinstated(Id));
        }

        private void Apply(AccountCreated e)
        {
            Id = e.AccountId;
            BusinessName = e.BusinessName;
            AccountNumber = e.AccountNumber;
        }

        private void Apply(SystemTagAdded e)
        {
            var systemTag = new SystemTag(e.Name, e.AppliesToExpenses, e.AppliesToTimesheets);
            var newList = SystemTags.ToList();
            newList.Add(systemTag);
            SystemTags = new ReadOnlyCollection<SystemTag>(newList);
        }

        private void Apply(AddressUpdated e)
        {
            Address = new Address(e.AddressLine1, e.AddressLine2, e.City, e.Postcode, e.State, e.CountryName);
        }

        private void Apply(AccountApproved e)
        {
            Status = new AccountStatus(true, e.ApprovedBy, false, null);
        }

        private void Apply(AccountDeleted e)
        {
            Status = new AccountStatus(Status.IsApproved, Status.ApprovedBy, true, e.Reason);
        }

        private void Apply(AccountReinstated e)
        {
            Status = new AccountStatus(Status.IsApproved, Status.ApprovedBy, false, null);
        }
    }
}