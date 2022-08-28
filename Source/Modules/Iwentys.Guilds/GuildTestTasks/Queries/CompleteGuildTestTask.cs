using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, AchievementProvider achievementProvider, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _achievementProvider = achievementProvider;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser review = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            GuildTestTaskSolution testTask = await _context.GuildTestTaskSolvingInfos.GetSingle(t => t.AuthorId == request.TaskSolveOwnerId && t.GuildId == request.GuildId);

            testTask.SetCompleted(review);
            _achievementProvider.AchieveForStudent(AchievementList.TestTaskDone, request.TaskSolveOwnerId);
            await AchievementHack.ProcessAchievement(_achievementProvider, _context);

            _context.GuildTestTaskSolvingInfos.Update(testTask);
            return new Response(GuildTestTaskInfoResponse.Wrap(testTask));
        }
    }
}