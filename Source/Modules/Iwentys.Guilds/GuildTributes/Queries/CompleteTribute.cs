using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds.Dtos;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildTributes.Queries
{
    public class CompleteTribute
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, TributeCompleteRequest arguments)
            {
                User = user;
                Arguments = arguments;
            }

            public AuthorizedUser User { get; set; }
            public TributeCompleteRequest Arguments { get; set; }
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
                IwentysUser student = _context.IwentysUsers.GetById(request.User.Id).Result;
                Tribute tribute = _context.Tributes.GetById(request.Arguments.TributeId).Result;
                Guild guild = await _context.Guilds.GetById(tribute.GuildId);
                GuildMentor mentor = student.EnsureIsGuildMentor(guild);

                tribute.SetCompleted(mentor.User.Id, request.Arguments);

                _context.Tributes.Update(tribute);
                return new Response(TributeInfoResponse.Wrap(tribute));
            }
        }
    }
}