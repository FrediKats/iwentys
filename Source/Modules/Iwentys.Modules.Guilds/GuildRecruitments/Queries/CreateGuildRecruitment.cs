using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds.Dtos;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildRecruitments.Queries
{
    public static class CreateGuildRecruitment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            public int GuildId { get; set; }
            public GuildRecruitmentCreateArguments Arguments { get; set; }

            public Query(AuthorizedUser authorizedUser, int guildId, GuildRecruitmentCreateArguments arguments)
            {
                AuthorizedUser = authorizedUser;
                GuildId = guildId;
                Arguments = arguments;
            }
        }

        public class Response
        {
            public Response(GuildRecruitmentInfoDto guildRecruitment)
            {
                GuildRecruitment = guildRecruitment;
            }

            public GuildRecruitmentInfoDto GuildRecruitment { get; set; }
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
                Guild guild = _context.Guilds.GetById(request.GuildId).Result;
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                var guildRecruitment = GuildRecruitment.Create(user, guild, request.Arguments);

                _context.GuildRecruitment.Add(guildRecruitment);
                return new Response(GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment));
            }
        }
    }
}