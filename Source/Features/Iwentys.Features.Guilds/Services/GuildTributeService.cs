using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Models.GuildTribute;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<GuildEntity> _guildRepositoryNew;
        private readonly IGenericRepository<GithubProjectEntity> _studentProjectRepository;
        private readonly IGenericRepository<TributeEntity> _guildTributeRepository;

        private readonly IGithubApiAccessor _githubApi;
        private readonly IGuildRepository _guildRepository;

        public GuildTributeService(IGithubApiAccessor githubApi, IGuildRepository guildRepository, IUnitOfWork unitOfWork)
        {
            _githubApi = githubApi;
            _guildRepository = guildRepository;
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepositoryNew = _unitOfWork.GetRepository<GuildEntity>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProjectEntity>();
            _guildTributeRepository = _unitOfWork.GetRepository<TributeEntity>();
        }

        //TODO: i'm not sure about this method
        public List<TributeInfoResponse> GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _guildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .GetAsync()
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.State == TributeState.Active)
                .AsEnumerable()
                .Select(TributeInfoResponse.Wrap)
                .ToList();
        }

        public TributeInfoResponse[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _guildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .GetAsync()
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.ProjectEntity.StudentId == user.Id)
                .AsEnumerable()
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public TributeInfoResponse[] GetGuildTributes(int guildId)
        {
            return _guildTributeRepository
                .GetAsync()
                .Where(t => t.GuildId == guildId)
                .Where(t => t.State == TributeState.Active)
                .AsEnumerable()
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TributeInfoResponse> CreateTribute(AuthorizedUser user, CreateProjectRequestDto createProject)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.Tribute.TributeCanBeSendFromStudentAccount(student.Id, createProject.Owner);

            GithubRepositoryInfoDto githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = await GetOrCreateAsync(githubProject, student);
            GuildEntity guild = _guildRepository.ReadForStudent(student.Id);
            List<TributeEntity> allTributes = await _guildTributeRepository.GetAsync().ToListAsync();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.Tribute.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.Tribute.UserAlreadyHaveTribute(user.Id);

            var tribute = new TributeEntity(guild, projectEntity);
            await _guildTributeRepository.InsertAsync(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }

        public async Task<GithubProjectEntity> GetOrCreateAsync(GithubRepositoryInfoDto project, StudentEntity creator)
        {
            GithubProjectEntity githubProjectEntity = await _studentProjectRepository.GetByIdAsync(project.Id);
            if (githubProjectEntity is not null)
                return githubProjectEntity;

            var newProject = new GithubProjectEntity(creator, project);
            await _studentProjectRepository.InsertAsync(newProject);
            await _unitOfWork.CommitAsync();
            return newProject;
        }


        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            TributeEntity tribute = await _guildTributeRepository.GetByIdAsync(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(tribute.ProjectId);

            if (tribute.ProjectEntity.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                await student.EnsureIsMentor(_guildRepositoryNew, tribute.GuildId);
                tribute.SetCanceled();
            }

            _guildTributeRepository.Update(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }

        public async Task<TributeInfoResponse> CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            TributeEntity tribute = await _guildTributeRepository.GetByIdAsync(tributeCompleteRequest.TributeId);
            GuildMentorUser mentor = await student.EnsureIsMentor(_guildRepositoryNew, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(tribute.ProjectId);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteRequest.DifficultLevel, tributeCompleteRequest.Mark);
            _guildTributeRepository.Update(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }
    }
}