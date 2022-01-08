using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Karmas;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Gamification;

public static class SendKarma
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
            IwentysUser author = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
            IwentysUser target = await _context.IwentysUsers.GetById(request.StudentId);

            var karmaUpVote = KarmaUpVote.Create(author, target);

            _context.KarmaUpVotes.Add(karmaUpVote);

            return new Response();
        }
    }
}