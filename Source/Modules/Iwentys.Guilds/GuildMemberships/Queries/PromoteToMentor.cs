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

public class PromoteToMentor
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }
        public int MemberId { get; set; }

        public Query(AuthorizedUser user, int guildId, int memberId)
        {
            User = user;
            GuildId = guildId;
            MemberId = memberId;
        }
    }

    public class Response
    {

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
            IwentysUser studentCreator = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            Guild guild = await _context.Guilds.GetById(request.GuildId);
            GuildCreator guildCreator = studentCreator.EnsureIsCreator(guild);

            GuildMember studentMembership = _context
                .GuildMembers
                .Where(GuildMember.IsMember())
                .Single(gm => gm.MemberId == request.MemberId && gm.GuildId == request.GuildId);
            studentMembership.MakeMentor(guildCreator);

            _context.GuildMembers.Update(studentMembership);
            return new Response();
        }
    }
}