using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            //TODO: move to domain
            IReadOnlyCollection<IwentysUser> users = await _entityManagerApiClient.IwentysUserProfiles.GetAsync();
            bool isUsernameUsed = users.Any(s => s.GithubUsername == request.StudentUpdateRequest.GithubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.StudentExceptions.GithubAlreadyUser(request.StudentUpdateRequest.GithubUsername);

            //throw new NotImplementedException("Need to validate github credentials");
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            user.GithubUsername = request.StudentUpdateRequest.GithubUsername;
            _entityManagerApiClient.IwentysUserProfiles.Update(user);

            //await _achievementProvider.AchieveForStudent(AchievementList.AddGithubAchievement, user.Id);

            return new Response();
        }
    }
}