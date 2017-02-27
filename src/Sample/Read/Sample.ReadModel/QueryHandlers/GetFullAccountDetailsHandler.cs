using System.Linq;
using Sample.Queries;
using Sample.Queries.Responses;
using Ximo.Cqrs;
using Ximo.Validation;

namespace Sample.ReadModel.QueryHandlers
{
    internal class GetFullAccountDetailsHandler :
        IQueryHandler<GetAccountDetailsById, GetAccountDetailsByIdResponse>
    {
        private readonly ReadModelContext _context;

        public GetFullAccountDetailsHandler(ReadModelContext context)
        {
            Check.NotNull(context, nameof(context));
            _context = context;
        }

        public GetAccountDetailsByIdResponse Read(GetAccountDetailsById query)
        {
            Check.NotNull(query, nameof(query));

            var accountDetails = _context.AccountDetails.First(x => x.AccountId == query.AccountId);
            var systemTags = _context.SystemTags.Where(x => x.AccountId == query.AccountId).ToList();

            return new GetAccountDetailsByIdResponse(accountDetails.ToAccountDetailsDto(),
                systemTags.Select(s => s.ToSystemTagDto()));
        }
    }
}