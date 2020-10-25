using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Gamification;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildTestTaskService : IGuildTestTaskService
    {
        private readonly DatabaseAccessor _database;
        private readonly IGithubApiAccessor _githubApi;
        private readonly AchievementProvider _achievementProvider;

        public GuildTestTaskService(DatabaseAccessor database, IGithubApiAccessor githubApi, AchievementProvider achievementProvider)
        {
            _database = database;
            _githubApi = githubApi;
            _achievementProvider = achievementProvider;
        }

        public List<GuildTestTaskInfoResponse> Get(int guildId)
        {
            return _database
                .GuildTestTaskSolvingInfo
                .Read()
                .Where(t => t.GuildId == guildId)
                .AsEnumerable()
                .Select(GuildTestTaskInfoResponse.Wrap)
                .ToList();
        }

        //TODO: check if already accepted
        public GuildTestTaskInfoResponse Accept(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _database.Guild.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            return _database.GuildTestTaskSolvingInfo
                .Create(studentGuild, user.GetProfile(_database.Student))
                .To(GuildTestTaskInfoResponse.Wrap);
        }

        //TODO: ensure project belong to user
        public GuildTestTaskInfoResponse Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolvingInfoEntity testTask = _database.GuildTestTaskSolvingInfo
                .Read()
                .SingleOrDefault(t => t.StudentId == user.Id && t.GuildId == guildId)?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            //TODO: replace with call to db cache
            GithubRepository githubRepository = _githubApi.GetRepository(projectOwner, projectName);
            testTask.SendSubmit(githubRepository.Id);

            return _database.GuildTestTaskSolvingInfo
                .Update(testTask)
                .To(GuildTestTaskInfoResponse.Wrap);
        }

        public GuildTestTaskInfoResponse Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            StudentEntity review = user.GetProfile(_database.Student);
            review.EnsureIsMentor(_database.Guild, guildId);

            GuildTestTaskSolvingInfoEntity testTask = _database.GuildTestTaskSolvingInfo
                .Read()
                .SingleOrDefault(t => t.StudentId == taskSolveOwnerId && t.GuildId == guildId) ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            _achievementProvider.Achieve(AchievementList.TestTaskDone, taskSolveOwnerId);

            return _database.GuildTestTaskSolvingInfo
                .Update(testTask)
                .To(GuildTestTaskInfoResponse.Wrap);
        }
    }
}