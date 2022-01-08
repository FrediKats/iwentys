using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildTestTasks.Queries
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

            public Handler(IwentysDbContext context, AchievementProvider achievementProvider)
            {
                _context = context;
                _achievementProvider = achievementProvider;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser review = await _context.IwentysUsers.GetById(request.User.Id);
                GuildTestTaskSolution testTask = await _context.GuildTestTaskSolvingInfos.GetSingle(t => t.AuthorId == request.TaskSolveOwnerId && t.GuildId == request.GuildId);

                testTask.SetCompleted(review);
                _achievementProvider.AchieveForStudent(AchievementList.TestTaskDone, request.TaskSolveOwnerId);
                await AchievementHack.ProcessAchievement(_achievementProvider, _context);

                _context.GuildTestTaskSolvingInfos.Update(testTask);
                return new Response(GuildTestTaskInfoResponse.Wrap(testTask));
            }
        }
    }
}