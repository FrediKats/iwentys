using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Newsfeeds;

public class GetGuildNewsfeeds
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser AuthorizedUser { get; }
        public int GuildId { get; }

        public Query(AuthorizedUser authorizedUser, int guildId)
        {
            AuthorizedUser = authorizedUser;
            GuildId = guildId;
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
                .GuildNewsfeeds
                .Where(gn => gn.GuildId == request.GuildId)
                .Select(NewsfeedViewModel.FromGuildEntity)
                .ToListAsync();

            return new Response(result);
        }
    }
}