namespace Ximo.Cqrs.Security
{
    /// <summary>
    ///     Defines the contract for an authorization test for an authenticated user trying to execute an operation.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IAuthorizationRule<in TMessage>
        where TMessage : IMessage
    {
        /// <summary>
        ///     Gets the error text.
        /// </summary>
        /// <value>The error text.</value>
        string ErrorText { get; }

        /// <summary>
        ///     Determines whether the specified message execution is authorized.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if the specified message execution is authorized; otherwise, <c>false</c>.</returns>
        bool IsAuthorized(TMessage message);
    }
}