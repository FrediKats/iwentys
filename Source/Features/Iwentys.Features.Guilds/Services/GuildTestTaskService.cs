using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
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
        private readonly IGenericRepository<GuildMemberEntity> _guildMemberRepository;
        private readonly IGenericRepository<GuildTestTaskSolutionEntity> _guildTestTaskSolvingInfoRepository;

        private readonly AchievementProvider _achievementProvider;
        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildTestTaskService(AchievementProvider achievementProvider, IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
        {
            _achievementProvider = achievementProvider;

            _unitOfWork = unitOfWork;
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepositoryNew = _unitOfWork.GetRepository<GuildEntity>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMemberEntity>();
            _guildTestTaskSolvingInfoRepository = _unitOfWork.GetRepository<GuildTestTaskSolutionEntity>();
        }

        public Task<List<GuildTestTaskInfoResponse>> Get(int guildId)
        {
            return _guildTestTaskSolvingInfoRepository
                .Get()
                .Where(t => t.GuildId == guildId)
                .Select(GuildTestTaskInfoResponse.FromEntity)
                .ToListAsync();
        }

        public async Task<GuildTestTaskInfoResponse> Accept(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);


            StudentEntity studentProfile = await _studentRepository.FindByIdAsync(user.Id);

            var existedTestTask = await _guildTestTaskSolvingInfoRepository
                .Get()
                .FirstOrDefaultAsync(k =>
                    k.GuildId == studentGuild.Id &&
                    k.StudentId == user.Id &&
                    k.GetState() != GuildTestTaskState.Completed);

            if (existedTestTask is not null)
                InnerLogicException.Guild.ActiveTestExisted(user.Id, guildId);

            var testTaskResonse = GuildTestTaskSolutionEntity.Create(studentGuild, studentProfile);
            await _guildTestTaskSolvingInfoRepository.InsertAsync(testTaskResonse);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTaskResonse);
        }

        //TODO: ensure project belong to user
        public async Task<GuildTestTaskInfoResponse> Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolutionEntity testTask = await _guildTestTaskSolvingInfoRepository
                                                          .Get()
                                                          .SingleOrDefaultAsync(t => t.StudentId == user.Id && t.GuildId == guildId)
                                                      ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            GithubRepositoryInfoDto githubRepositoryInfoDto = await _githubIntegrationService.GetRepository(projectOwner, projectName);
            testTask.SendSubmit(githubRepositoryInfoDto.Id);

            _guildTestTaskSolvingInfoRepository.Update(testTask);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTask);
        }

        public async Task<GuildTestTaskInfoResponse> Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            StudentEntity review = await _studentRepository.FindByIdAsync(user.Id);
            await review.EnsureIsMentor(_guildRepositoryNew, guildId);

            GuildTestTaskSolutionEntity testTask = _guildTestTaskSolvingInfoRepository
                .Get()
                .SingleOrDefault(t => t.StudentId == taskSolveOwnerId && t.GuildId == guildId) ?? throw new EntityNotFoundException("Test task was not started");

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            await _achievementProvider.Achieve(AchievementList.TestTaskDone, taskSolveOwnerId);

            _guildTestTaskSolvingInfoRepository.Update(testTask);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTask);
        }
    }
}