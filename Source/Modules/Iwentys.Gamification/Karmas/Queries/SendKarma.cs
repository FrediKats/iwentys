using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Karmas;
using Iwentys.EntityManagerServiceIntegration;
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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser author = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            IwentysUser target = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.StudentId);

            var karmaUpVote = KarmaUpVote.Create(author, target);

            _context.KarmaUpVotes.Add(karmaUpVote);

            return new Response();
        }
    }
}