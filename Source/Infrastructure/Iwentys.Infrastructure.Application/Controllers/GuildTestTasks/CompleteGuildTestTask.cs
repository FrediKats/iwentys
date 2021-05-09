using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildTestTasks
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
            private readonly IwentysDbContext _context;
            private readonly AchievementProvider _achievementProvider;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IwentysDbContext context, AchievementProvider achievementProvider, IUnitOfWork unitOfWork)
            {
                _context = context;
                _achievementProvider = achievementProvider;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser review = await _context.IwentysUsers.GetById(request.User.Id);
                GuildTestTaskSolution testTask = await _context.GuildTestTaskSolvingInfos.GetSingle(t => t.AuthorId == request.TaskSolveOwnerId && t.GuildId == request.GuildId);

                testTask.SetCompleted(review);
                _achievementProvider.AchieveForStudent(AchievementList.TestTaskDone, request.TaskSolveOwnerId);
                await AchievementHack.ProcessAchievement(_achievementProvider, _context);

                _context.GuildTestTaskSolvingInfos.Update(testTask);
                await _unitOfWork.CommitAsync();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTask));
            }
        }
    }
}