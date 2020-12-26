using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Tributes.Entities;
using Iwentys.Features.Tributes.Enums;
using Iwentys.Features.Tributes.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Tributes.Services
{
    public class GuildTributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<GuildEntity> _guildRepositoryNew;
        private readonly IGenericRepository<GuildMemberEntity> _guildMemberRepository;
        private readonly IGenericRepository<GithubProjectEntity> _studentProjectRepository;
        private readonly IGenericRepository<TributeEntity> _guildTributeRepository;

        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildTributeService(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
        {
            _unitOfWork = unitOfWork;
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepositoryNew = _unitOfWork.GetRepository<GuildEntity>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMemberEntity>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProjectEntity>();
            _guildTributeRepository = _unitOfWork.GetRepository<TributeEntity>();
        }

        public List<TributeInfoResponse> GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _guildMemberRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .Get()
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Where(t => t.Project != null)
                .Select(TributeInfoResponse.FromEntity)
                .ToList();
        }

        public List<TributeInfoResponse> GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _guildMemberRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .Get()
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.Project.StudentId == user.Id)
                .Select(TributeInfoResponse.FromEntity)
                .ToList();
        }

        public List<TributeInfoResponse> GetGuildTributes(int guildId)
        {
            return _guildTributeRepository
                .Get()
                .Where(t => t.GuildId == guildId)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoResponse.FromEntity)
                .ToList();
        }

        public async Task<TributeInfoResponse> CreateTribute(AuthorizedUser user, CreateProjectRequestDto createProject)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            GithubRepositoryInfoDto githubProject = await _githubIntegrationService.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = await GetOrCreateAsync(githubProject, student);
            GuildEntity guild = _guildMemberRepository.ReadForStudent(student.Id);
            List<TributeEntity> allTributes = await _guildTributeRepository.Get().ToListAsync();

            var tribute = TributeEntity.Create(guild, student, projectEntity, allTributes);

            await _guildTributeRepository.InsertAsync(tribute);
            await _unitOfWork.CommitAsync();

            //TODO: remove this hack, check issue https://github.com/kysect/iwentys/issues/138
            //return await _guildTributeRepository
            //    .Get()
            //    .Where(t => t.ProjectId == tribute.ProjectId)
            //    .Select(TributeInfoResponse.FromEntity)
            //    .SingleAsync();

            tribute.Project = projectEntity;
            return TributeInfoResponse.Wrap(tribute);
            
        }

        public async Task<GithubProjectEntity> GetOrCreateAsync(GithubRepositoryInfoDto project, StudentEntity creator)
        {
            GithubProjectEntity githubProjectEntity = await _studentProjectRepository.FindByIdAsync(project.Id);
            if (githubProjectEntity is not null)
                return githubProjectEntity;

            //TODO: need to get this from GithubService
            var newProject = new GithubProjectEntity(creator, project);
            
            await _studentProjectRepository.InsertAsync(newProject);
            await _unitOfWork.CommitAsync();
            return newProject;
        }


        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            TributeEntity tribute = await _guildTributeRepository.FindByIdAsync(tributeId);

            if (tribute.Project.StudentId == user.Id)
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
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            TributeEntity tribute = await _guildTributeRepository.FindByIdAsync(tributeCompleteRequest.TributeId);
            GuildMentorUser mentor = await student.EnsureIsMentor(_guildRepositoryNew, tribute.GuildId);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteRequest);
            
            _guildTributeRepository.Update(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }

        public async Task<TributeInfoResponse> FindStudentActiveTribute(AuthorizedUser user)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            return await _guildTributeRepository
                .Get()
                .Where(TributeEntity.IsActive)
                .Where(TributeEntity.BelongTo(student))
                .Select(TributeInfoResponse.FromEntity)
                .SingleOrDefaultAsync();
        }
    }
}