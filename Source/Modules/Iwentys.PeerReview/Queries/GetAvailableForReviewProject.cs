using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.PeerReview.Queries
{
    public static class GetAvailableForReviewProject
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
            public List<GithubRepositoryInfoDto> Result { get; set; }

            public Response(List<GithubRepositoryInfoDto> result)
            {
                Result = result;
            }
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
                HashSet<long> userProjects = _context
                    .ProjectReviewRequests
                    .Where(k => k.AuthorId == request.AuthorizedUser.Id)
                    .SelectToHashSet(k => k.ProjectId);

                List<GithubRepositoryInfoDto> result = await _context
                    .StudentProjects
                    .Where(p => p.OwnerUserId == request.AuthorizedUser.Id && !userProjects.Contains(p.Id))
                    .Select(GithubRepositoryInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}