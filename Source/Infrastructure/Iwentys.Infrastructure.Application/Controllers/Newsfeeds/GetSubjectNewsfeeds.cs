using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Newsfeeds.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Newsfeeds
{
    public class GetSubjectNewsfeeds
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; }
            public int SubjectId { get; }

            public Query(AuthorizedUser authorizedUser, int subjectId)
            {
                AuthorizedUser = authorizedUser;
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public List<NewsfeedViewModel> Newsfeeds { get; set; }

            public Response(List<NewsfeedViewModel> newsfeeds)
            {
                Newsfeeds = newsfeeds;
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
                List<NewsfeedViewModel> result = await _context
                    .SubjectNewsfeeds
                    .Where(sn => sn.SubjectId == request.SubjectId)
                    .Select(NewsfeedViewModel.FromSubjectEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}