using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.InterestTags;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement;

public class GetAllTags
{
    public class Query : IRequest<Response>
    {
        public Query()
        {
        }
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
            List<InterestTag> interestTagEntities = await _context.InterestTags.ToListAsync();
            return new Response(interestTagEntities.SelectToList(t => new InterestTagDto(t)));
        }
    }
}