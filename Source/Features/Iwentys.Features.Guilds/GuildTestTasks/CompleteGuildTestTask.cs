using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.FeatureBase;
using MediatR;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public static class CompleteGuildTestTask
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId, AuthorizedUser user, int taskSolveOwnerId)
            {
                GuildId = guildId;
                User = user;
                TaskSolveOwnerId = taskSolveOwnerId;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public int TaskSolveOwnerId { get; set; }
        }

        public class Response
        {
            public Response(GuildTestTaskInfoResponse testTaskInfo)
            {
                TestTaskInfo = testTaskInfo;
            }

            public GuildTestTaskInfoResponse TestTaskInfo { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly AchievementProvider _achievementProvider;

            private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;

            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<IwentysUser> _userRepository;

            public Handler(IUnitOfWork unitOfWork, AchievementProvider achievementProvider)
            {
                _achievementProvider = achievementProvider;

                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser review = await _userRepository.GetById(request.User.Id);
                GuildTestTaskSolution testTask = await _guildTestTaskSolutionRepository.GetSingle(t => t.AuthorId == request.TaskSolveOwnerId && t.GuildId == request.GuildId);

                testTask.SetCompleted(review);
                _achievementProvider.AchieveForStudent(AchievementList.TestTaskDone, request.TaskSolveOwnerId);
                await AchievementHack.ProcessAchievement(_achievementProvider, _unitOfWork);

                _guildTestTaskSolutionRepository.Update(testTask);
                await _unitOfWork.CommitAsync();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTask));
            }
        }
    }
}