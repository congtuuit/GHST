using GHSTShipping.Application.DTOs.User;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Users.Queries
{
    public class GetNoticesRequest : IRequest<BaseResult<IEnumerable<NoticeDto>>>
    {
    }

    public class GetNoticesRequestHandler : IRequestHandler<GetNoticesRequest, BaseResult<IEnumerable<NoticeDto>>>
    {

        public GetNoticesRequestHandler() { }

        public async Task<BaseResult<IEnumerable<NoticeDto>>> Handle(GetNoticesRequest request, CancellationToken cancellationToken)
        {
            var notices = new List<NoticeDto>();

            return BaseResult<IEnumerable<NoticeDto>>.Ok(notices);
        }
    }
}
