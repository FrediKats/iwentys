using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Features.Guilds.Services;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tributes.Services
{
    public class GuildTributeService
    {
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepositoryNew;
        private readonly IGenericRepository<Tribute> _guildTributeRepository;
        private readonly IGenericRepository<GithubProject> _studentProjectRepository;

        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GuildTributeService(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
        {
            _unitOfWork = unitOfWork;
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepositoryNew = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
            _guildTributeRepository = _unitOfWork.GetRepository<Tribute>();
        }

        //TODO: seems like it all will not work if user leave from guild, join other and resend tribute for project
        public async Task<TributeInfoResponse> Get(AuthorizedUser user, int tributeId)
        {
            Guild guild = _guildMemberRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, null);

            return await _guildTributeRepository
                .Get()
                .Where(t => t.ProjectId == tributeId)
                .Select(TributeInfoResponse.FromEntity)
                .FirstAsync();
        }

        public List<TributeInfoResponse> GetPendingTributes(AuthorizedUser user)
        {
            Guild guild = _guildMemberRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, null);

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
            Guild guild = _guildMemberRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .Get()
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.Project.OwnerUserId == user.Id)
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
            IwentysUser student = await _studentRepository.FindByIdAsync(user.Id);
            GithubRepositoryInfoDto githubProject = await _githubIntegrationService.Repository.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProject project = await GetOrCreate(githubProject, student);
            Guild guild = _guildMemberRepository.ReadForStudent(student.Id);
            List<Tribute> allTributes = await _guildTributeRepository.Get().ToListAsync();

            var tribute = Tribute.Create(guild, student, project, allTributes);

            await _guildTributeRepository.InsertAsync(tribute);
            await _unitOfWork.CommitAsync();

            return await _guildTributeRepository
                .Get()
                .Where(t => t.ProjectId == tribute.ProjectId)
                .Select(TributeInfoResponse.FromEntity)
                .SingleAsync();
        }

        //TODO: looks like hack or method from other service
        public async Task<GithubProject> GetOrCreate(GithubRepositoryInfoDto project, IwentysUser creator)
        {
            GithubProject githubProject = await _studentProjectRepository.FindByIdAsync(project.Id);
            if (githubProject is not null)
                return githubProject;

            GithubUser githubUser = await _githubIntegrationService.User.Get(creator.Id);
            //TODO: need to get this from GithubService
            var newProject = new GithubProject(githubUser, project);

            await _studentProjectRepository.InsertAsync(newProject);
            await _unitOfWork.CommitAsync();
            return newProject;
        }


        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            IwentysUser student = await _studentRepository.FindByIdAsync(user.Id);
            Tribute tribute = await _guildTributeRepository.FindByIdAsync(tributeId);

            if (tribute.Project.OwnerUserId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                await student.EnsureIsGuildMentor(_guildRepositoryNew, tribute.GuildId);
                tribute.SetCanceled();
            }

            _guildTributeRepository.Update(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }

        public async Task<TributeInfoResponse> CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest)
        {
            IwentysUser student = await _studentRepository.FindByIdAsync(user.Id);
            Tribute tribute = await _guildTributeRepository.FindByIdAsync(tributeCompleteRequest.TributeId);
            GuildMentor mentor = await student.EnsureIsGuildMentor(_guildRepositoryNew, tribute.GuildId);

            tribute.SetCompleted(mentor.User.Id, tributeCompleteRequest);

            _guildTributeRepository.Update(tribute);
            await _unitOfWork.CommitAsync();
            return TributeInfoResponse.Wrap(tribute);
        }

        public async Task<TributeInfoResponse> FindStudentActiveTribute(AuthorizedUser user)
        {
            IwentysUser student = await _studentRepository.FindByIdAsync(user.Id);
            return await _guildTributeRepository
                .Get()
                .Where(Tribute.IsActive)
                .Where(Tribute.BelongTo(student))
                .Select(TributeInfoResponse.FromEntity)
                .SingleOrDefaultAsync();
        }
    }
}