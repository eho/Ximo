using System;
using Sample.Queries.Responses;
using Ximo.Cqrs;

namespace Sample.Queries
{
    public class GetAccountDetailsById : IQuery<GetAccountDetailsByIdResponse>
    {
        private GetAccountDetailsById()
        {
        }

        public GetAccountDetailsById(Guid accountId) : this()
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}