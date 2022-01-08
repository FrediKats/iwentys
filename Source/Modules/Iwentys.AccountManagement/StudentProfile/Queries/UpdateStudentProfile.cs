using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement
{
    public class UpdateStudentProfile
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, StudentUpdateRequestDto studentUpdateRequest)
            {
                AuthorizedUser = authorizedUser;
                StudentUpdateRequest = studentUpdateRequest;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public StudentUpdateRequestDto StudentUpdateRequest { get; set; }
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
                //TODO: move to domain
                bool isUsernameUsed = await _context.IwentysUsers.AnyAsync(s => s.GithubUsername == request.StudentUpdateRequest.GithubUsername);
                if (isUsernameUsed)
                    throw InnerLogicException.StudentExceptions.GithubAlreadyUser(request.StudentUpdateRequest.GithubUsername);

                //throw new NotImplementedException("Need to validate github credentials");
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                user.GithubUsername = request.StudentUpdateRequest.GithubUsername;
                _context.IwentysUsers.Update(user);

                //await _achievementProvider.AchieveForStudent(AchievementList.AddGithubAchievement, user.Id);

                return new Response();
            }
        }
    }
}