using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.InterestTags;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.AccountManagement.InterestTags.Queries
{
    public class GetUserTags
    {
        public class Query : IRequest<Response>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; set; }
        }

        public class Response
        {
            public Response(List<InterestTagDto> tags)
            {
                Tags = tags;
            }

            public List<InterestTagDto> Tags { get; set; }
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
                List<InterestTagDto> result = await _context
                    .UserInterestTags
                    .Where(ui => ui.UserId == request.UserId)
                    .Select(ui => ui.InterestTag)
                    .Select(InterestTagDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}