using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildTestTaskService : IGuildTestTaskService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IGithubApiAccessor _githubApi;

        public GuildTestTaskService(DatabaseAccessor databaseAccessor, IGithubApiAccessor githubApi)
        {
            _databaseAccessor = databaseAccessor;
            _githubApi = githubApi;
        }

        public List<GuildTestTaskInfoDto> Get(int guildId)
        {
            return _databaseAccessor
                .GuildTestTaskSolvingInfo
                .Read()
                .Where(t => t.GuildId == guildId)
                .AsEnumerable()
                .Select(GuildTestTaskInfoDto.Wrap)
                .ToList();
        }

        //TODO: check if already accepted
        public GuildTestTaskInfoDto Accept(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _databaseAccessor.GuildRepository.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            return _databaseAccessor.GuildTestTaskSolvingInfo
                .Create(studentGuild, user.GetProfile(_databaseAccessor.Student))
                .To(GuildTestTaskInfoDto.Wrap);
        }

        //TODO: ensure project belong to user
        public GuildTestTaskInfoDto Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolvingInfoEntity testTask = _databaseAccessor.GuildTestTaskSolvingInfo
                .Read()
                .SingleOrDefault(t => t.StudentId == user.Id && t.GuildId == guildId)?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            //TODO: replace with call to db cache
            GithubRepository githubRepository = _githubApi.GetRepository(projectOwner, projectName);
            testTask.SendSubmit(githubRepository.Id);

            return _databaseAccessor.GuildTestTaskSolvingInfo
                .Update(testTask)
                .To(GuildTestTaskInfoDto.Wrap);
        }

        public GuildTestTaskInfoDto Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            StudentEntity review = user.GetProfile(_databaseAccessor.Student);
            review.EnsureIsMentor(_databaseAccessor.GuildRepository, guildId);

            GuildTestTaskSolvingInfoEntity testTask = _databaseAccessor.GuildTestTaskSolvingInfo
                .Read()
                .SingleOrDefault(t => t.StudentId == taskSolveOwnerId && t.GuildId == guildId) ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            return _databaseAccessor.GuildTestTaskSolvingInfo
                .Update(testTask)
                .To(GuildTestTaskInfoDto.Wrap);
        }
    }
}