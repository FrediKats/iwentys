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

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Guild> _guildRepositoryNew;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolvingInfoRepository;

        private readonly AchievementProvider _achievementProvider;
        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildTestTaskService(AchievementProvider achievementProvider, IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
        {
            _achievementProvider = achievementProvider;

            _unitOfWork = unitOfWork;
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _guildRepositoryNew = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildTestTaskSolvingInfoRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
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
            Guild studentGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, guildId);


            Student studentProfile = await _studentRepository.FindByIdAsync(user.Id);

            var existedTestTask = await _guildTestTaskSolvingInfoRepository
                .Get()
                .FirstOrDefaultAsync(k =>
                    k.GuildId == studentGuild.Id &&
                    k.StudentId == user.Id &&
                    k.GetState() != GuildTestTaskState.Completed);

            if (existedTestTask is not null)
                InnerLogicException.GuildExceptions.ActiveTestExisted(user.Id, guildId);

            var testTaskResonse = GuildTestTaskSolution.Create(studentGuild, studentProfile);
            await _guildTestTaskSolvingInfoRepository.InsertAsync(testTaskResonse);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTaskResonse);
        }

        //TODO: ensure project belong to user
        public async Task<GuildTestTaskInfoResponse> Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            GuildTestTaskSolution testTask = await _guildTestTaskSolvingInfoRepository
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
            Student review = await _studentRepository.FindByIdAsync(user.Id);
            await review.EnsureIsGuildMentor(_guildRepositoryNew, guildId);

            GuildTestTaskSolution testTask = _guildTestTaskSolvingInfoRepository
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