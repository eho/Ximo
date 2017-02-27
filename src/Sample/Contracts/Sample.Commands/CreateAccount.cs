using System;
using Ximo.Cqrs;

namespace Sample.Commands
{
    public class CreateAccount : ICommand
    {
        private CreateAccount()
        {
        }

        public CreateAccount(Guid newAccountId, string firstName, string lastName, string businessName, string userEmail)
            : this()
        {
            NewAccountId = newAccountId;
            FirstName = firstName;
            LastName = lastName;
            BusinessName = businessName;
            UserEmail = userEmail;
        }

        public Guid NewAccountId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string BusinessName { get; private set; }
        public string UserEmail { get; private set; }
    }
}