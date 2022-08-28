using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class CancelTribute
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user, long tributeId)
        {
            User = user;
            TributeId = tributeId;
        }

        public AuthorizedUser User { get; set; }
        public long TributeId { get; set; }
    }

    public class Response
    {
        public Response(TributeInfoResponse tribute)
        {
            Tribute = tribute;
        }

        public TributeInfoResponse Tribute { get; set; }
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
            IwentysUser student = _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id).Result;
            Tribute tribute = _context.Tributes.GetById(request.TributeId).Result;

            if (tribute.Project.OwnerUserId == request.User.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                //TODO: move logic to domain
                Guild guild = await _context.Guilds.GetById(tribute.GuildId);
                student.EnsureIsGuildMentor(guild);
                tribute.SetCanceled();
            }

            _context.Tributes.Update(tribute);
            return new Response(TributeInfoResponse.Wrap(tribute));
        }
    }
}