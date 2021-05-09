using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain.InterestTags;
using Iwentys.Domain.InterestTags.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.InterestTags
{
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
}