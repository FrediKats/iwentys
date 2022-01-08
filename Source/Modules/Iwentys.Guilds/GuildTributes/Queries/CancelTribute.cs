using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
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

        public Handler(IwentysDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser student = _context.Students.GetById(request.User.Id).Result;
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