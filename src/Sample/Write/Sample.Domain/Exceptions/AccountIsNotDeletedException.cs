using System;

namespace Sample.Domain.Exceptions
{
    public class AccountIsNotDeletedException : Exception
    {
        internal AccountIsNotDeletedException(string message)
            : base(message)
        {
        }

        internal static AccountIsNotDeletedException Create()
        {
            var message = "The account cannot be reinstated as it has not been deleted in the first place :)";
            return new AccountIsNotDeletedException(message);
        }
    }
}