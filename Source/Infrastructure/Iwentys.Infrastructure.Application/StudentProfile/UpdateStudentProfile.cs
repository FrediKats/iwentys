using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.StudentProfile
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
            private readonly IUnitOfWork _unitOfWork;

            private readonly IGenericRepository<IwentysUser> _userRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                //TODO: move to domain
                bool isUsernameUsed = await _userRepository.Get().AnyAsync(s => s.GithubUsername == request.StudentUpdateRequest.GithubUsername);
                if (isUsernameUsed)
                    throw InnerLogicException.StudentExceptions.GithubAlreadyUser(request.StudentUpdateRequest.GithubUsername);

                //throw new NotImplementedException("Need to validate github credentials");
                IwentysUser user = await _userRepository.GetById(request.AuthorizedUser.Id);
                user.GithubUsername = request.StudentUpdateRequest.GithubUsername;
                _userRepository.Update(user);

                //await _achievementProvider.AchieveForStudent(AchievementList.AddGithubAchievement, user.Id);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}