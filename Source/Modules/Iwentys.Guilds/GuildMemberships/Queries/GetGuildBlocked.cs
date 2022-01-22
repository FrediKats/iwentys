using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class GetGuildBlocked
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }

        public Query(AuthorizedUser user, int guildId)
        {
            User = user;
            GuildId = guildId;
        }
    }

    public class Response
    {
        public List<GuildMember> GuildMembers { get; set; }

        public Response(List<GuildMember> guildMembers)
        {
            GuildMembers = guildMembers;
        }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser student = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            Guild guild = await _context.Guilds.GetById(request.GuildId);

            student.EnsureIsGuildMentor(guild);

            List<GuildMember> result = guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToList();

            return new Response(result);
        }
    }
}