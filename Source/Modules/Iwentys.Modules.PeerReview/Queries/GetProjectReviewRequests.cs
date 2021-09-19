using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.PeerReview.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.PeerReview.Queries
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
            private readonly IMapper _mapper;

            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }


            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                List<ProjectReviewRequestInfoDto> result = await _context
                    .ProjectReviewRequests
                    .Where(ProjectReviewRequest.IsVisibleTo(user))
                    .ProjectTo<ProjectReviewRequestInfoDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new Response(result);
            }
        }
    }
}