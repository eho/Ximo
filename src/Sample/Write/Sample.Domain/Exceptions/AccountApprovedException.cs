using System;

namespace Sample.Domain.Exceptions
{
    public class AccountApprovedException : Exception
    {
        internal AccountApprovedException(string message)
            : base(message)
        {
        }

        internal static AccountApprovedException Create()
        {
            var message = "The account is has been approved previously.";
            return new AccountApprovedException(message);
        }
    }
}