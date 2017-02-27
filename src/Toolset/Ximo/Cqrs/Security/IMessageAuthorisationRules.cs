using System.Collections.Generic;

namespace Ximo.Cqrs.Security
{
    public interface IMessageAuthorisationRules<in TMessage> where TMessage : IMessage
    {
        IEnumerable<IAuthorizationRule<TMessage>> Rules { get; }
    }
}