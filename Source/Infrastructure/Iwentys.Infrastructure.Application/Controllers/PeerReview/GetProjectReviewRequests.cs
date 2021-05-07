using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.PeerReview
{
    public class GetProjectReviewRequests
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser)
            {
                AuthorizedUser = authorizedUser;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
            public Response(List<ProjectReviewRequestInfoDto> requests)
            {
                Requests = requests;
            }

            public List<ProjectReviewRequestInfoDto> Requests { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }


            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                List<ProjectReviewRequestInfoDto> result = await _context
                    .ProjectReviewRequests
                    .Where(ProjectReviewRequest.IsVisibleTo(user))
                    .Select(ProjectReviewRequestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}