using System;

namespace Ximo.EventSourcing
{
    public class AggregateConcurrencyException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateConcurrencyException" /> class.
        /// </summary>
        public AggregateConcurrencyException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateConcurrencyException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AggregateConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateConcurrencyException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public AggregateConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}