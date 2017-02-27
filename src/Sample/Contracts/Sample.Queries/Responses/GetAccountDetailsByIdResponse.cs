using System.Collections.Generic;
using Sample.Queries.Dtos;

namespace Sample.Queries.Responses
{
    public class GetAccountDetailsByIdResponse
    {
        private GetAccountDetailsByIdResponse()
        {
        }

        public GetAccountDetailsByIdResponse(AccountDetailsDto accountDetailsDto, IEnumerable<SystemTagDto> systemTags)
            : this()
        {
            AccountDetailsDto = accountDetailsDto;
            SystemTags = systemTags;
        }

        public AccountDetailsDto AccountDetailsDto { get; private set; }
        public IEnumerable<SystemTagDto> SystemTags { get; private set; }
    }
}