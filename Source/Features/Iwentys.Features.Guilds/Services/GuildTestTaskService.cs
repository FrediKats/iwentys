using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.ViewModels;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTestTaskService
    {
        private readonly GuildRepositoriesScope _database;
        private readonly IGithubApiAccessor _githubApi;
        private readonly AchievementProvider _achievementProvider;
        private readonly IGuildTestTaskSolvingInfoRepository _guildTestTaskSolvingInfoRepository;

        public GuildTestTaskService(GuildRepositoriesScope database, IGithubApiAccessor githubApi, AchievementProvider achievementProvider, IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfoRepository)
        {
            _database = database;
            _githubApi = githubApi;
            _achievementProvider = achievementProvider;
            _guildTestTaskSolvingInfoRepository = guildTestTaskSolvingInfoRepository;
        }

        public List<GuildTestTaskInfoResponse> Get(int guildId)
        {
            return _guildTestTaskSolvingInfoRepository
                .Read()
                .Where(t => t.GuildId == guildId)
                .AsEnumerable()
                .Select(GuildTestTaskInfoResponse.Wrap)
                .ToList();
        }

        //TODO: check if already accepted
        public async Task<GuildTestTaskInfoResponse> Accept(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _database.Guild.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            StudentEntity studentProfile = await user.GetProfile(_database.Student);
            return _guildTestTaskSolvingInfoRepository
                .Create(studentGuild, studentProfile)
                .To(GuildTestTaskInfoResponse.Wrap);
        }

        //TODO: ensure project belong to user
        public async Task<GuildTestTaskInfoResponse> Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolvingInfoEntity testTask = await _guildTestTaskSolvingInfoRepository
                                                          .Read()
                                                          .SingleOrDefaultAsync(t => t.StudentId == user.Id && t.GuildId == guildId)
                                                      ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            //TODO: replace with call to db cache
            GithubRepository githubRepository = _githubApi.GetRepository(projectOwner, projectName);
            testTask.SendSubmit(githubRepository.Id);

            return _guildTestTaskSolvingInfoRepository
                .Update(testTask)
                .To(GuildTestTaskInfoResponse.Wrap);
        }

        public async Task<GuildTestTaskInfoResponse> Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            StudentEntity review = await user.GetProfile(_database.Student);
            await review.EnsureIsMentor(_database.Guild, guildId);

            GuildTestTaskSolvingInfoEntity testTask = _guildTestTaskSolvingInfoRepository
                .Read()
                .SingleOrDefault(t => t.StudentId == taskSolveOwnerId && t.GuildId == guildId) ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            _achievementProvider.Achieve(AchievementList.TestTaskDone, taskSolveOwnerId);

            return _guildTestTaskSolvingInfoRepository
                .Update(testTask)
                .To(GuildTestTaskInfoResponse.Wrap);
        }
    }
}