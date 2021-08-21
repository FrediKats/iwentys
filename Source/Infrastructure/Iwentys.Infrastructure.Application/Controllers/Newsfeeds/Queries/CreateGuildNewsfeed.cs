using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.Newsfeeds.Dto;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Newsfeeds
{
    public class CreateGuildNewsfeed
    {
        public class Query : IRequest<Response>
        {
            public NewsfeedCreateViewModel CreateViewModel { get; }
            public AuthorizedUser AuthorizedUser { get; }
            public int GuildId { get; }

            public Query(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int guildId)
            {
                CreateViewModel = createViewModel;
                AuthorizedUser = authorizedUser;
                GuildId = guildId;
            }
        }

        public class Response
        {
            public Response(NewsfeedViewModel newsfeeds)
            {
                Newsfeeds = newsfeeds;
            }

            public NewsfeedViewModel Newsfeeds { get; }
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
                Student author = await _context.Students.GetById(request.AuthorizedUser.Id);
                Guild guild = await _context.Guilds.GetById(request.GuildId);

                GuildMentor mentor = author.EnsureIsGuildMentor(guild);
                var newsfeedEntity = GuildNewsfeed.Create(request.CreateViewModel, mentor, guild);

                _context.GuildNewsfeeds.Add(newsfeedEntity);

                NewsfeedViewModel result = await _context
                    .Newsfeeds
                    .Where(n => n.Id == newsfeedEntity.NewsfeedId)
                    .Select(NewsfeedViewModel.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}