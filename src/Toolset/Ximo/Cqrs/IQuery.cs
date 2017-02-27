namespace Ximo.Cqrs
{
    /// <summary>
    ///     Defines a contract for any query sent throughout the system as specified in the CQRS pattern.
    /// </summary>
    // ReSharper disable once UnusedTypeParameter
    public interface IQuery<TResult> : IMessage
    {
    }
}