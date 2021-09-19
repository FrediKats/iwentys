using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Karmas;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Gamification.Karmas.Queries
{
    public static class RevokeKarma
    {
        public class Query : IRequest<Response>
        {
            public int StudentId { get; }
            public AuthorizedUser AuthorizedUser { get; }

            public Query(int studentId, AuthorizedUser authorizedUser)
            {
                StudentId = studentId;
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
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
                IwentysUser target = await _context.IwentysUsers.GetById(request.StudentId);
                KarmaUpVote upVote = await _context.KarmaUpVotes.FirstAsync(k => k.AuthorId == request.AuthorizedUser.Id && k.TargetId == target.Id, cancellationToken);

                _context.KarmaUpVotes.Remove(upVote);
                return new Response();
            }
        }
    }
}