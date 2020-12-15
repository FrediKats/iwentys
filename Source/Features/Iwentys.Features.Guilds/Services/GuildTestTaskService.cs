using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTestTaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<GuildEntity> _guildRepositoryNew;
        private readonly IGenericRepository<GuildTestTaskSolvingInfoEntity> _guildTestTaskSolvingInfoRepository;

        private readonly IGithubApiAccessor _githubApi;
        private readonly AchievementProvider _achievementProvider;
        private readonly IGuildRepository _guildRepository;

        public GuildTestTaskService(IGithubApiAccessor githubApi, AchievementProvider achievementProvider, IGuildRepository guildRepository, IUnitOfWork unitOfWork)
        {
            _githubApi = githubApi;
            _achievementProvider = achievementProvider;
            _guildRepository = guildRepository;

            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepositoryNew = _unitOfWork.GetRepository<GuildEntity>();
            _guildTestTaskSolvingInfoRepository = _unitOfWork.GetRepository<GuildTestTaskSolvingInfoEntity>();
        }

        public List<GuildTestTaskInfoResponse> Get(int guildId)
        {
            return _guildTestTaskSolvingInfoRepository
                .GetAsync()
                .Where(t => t.GuildId == guildId)
                .AsEnumerable()
                .SelectToList(GuildTestTaskInfoResponse.Wrap);
        }

        public async Task<GuildTestTaskInfoResponse> Accept(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);


            StudentEntity studentProfile = await _studentRepository.GetByIdAsync(user.Id);

            var existedTestTask = await _guildTestTaskSolvingInfoRepository
                .GetAsync()
                .FirstOrDefaultAsync(k =>
                    k.GuildId == studentGuild.Id &&
                    k.StudentId == user.Id &&
                    k.GetState() != GuildTestTaskState.Completed);

            if (existedTestTask is not null)
                InnerLogicException.Guild.ActiveTestExisted(user.Id, guildId);

            var testTaskResonse = GuildTestTaskSolvingInfoEntity.Create(studentGuild, studentProfile);
            await _guildTestTaskSolvingInfoRepository.InsertAsync(testTaskResonse);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTaskResonse);
        }

        //TODO: ensure project belong to user
        public async Task<GuildTestTaskInfoResponse> Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolvingInfoEntity testTask = await _guildTestTaskSolvingInfoRepository
                                                          .GetAsync()
                                                          .SingleOrDefaultAsync(t => t.StudentId == user.Id && t.GuildId == guildId)
                                                      ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            //TODO: replace with call to db cache
            GithubRepositoryInfoDto githubRepositoryInfoDto = _githubApi.GetRepository(projectOwner, projectName);
            testTask.SendSubmit(githubRepositoryInfoDto.Id);

            await _guildTestTaskSolvingInfoRepository.UpdateAsync(testTask);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTask);
        }

        public async Task<GuildTestTaskInfoResponse> Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            StudentEntity review = await _studentRepository.GetByIdAsync(user.Id);
            await review.EnsureIsMentor(_guildRepositoryNew, guildId);

            GuildTestTaskSolvingInfoEntity testTask = _guildTestTaskSolvingInfoRepository
                .GetAsync()
                .SingleOrDefault(t => t.StudentId == taskSolveOwnerId && t.GuildId == guildId) ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            await _achievementProvider.Achieve(AchievementList.TestTaskDone, taskSolveOwnerId);

            await _guildTestTaskSolvingInfoRepository.UpdateAsync(testTask);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTask);
        }
    }
}