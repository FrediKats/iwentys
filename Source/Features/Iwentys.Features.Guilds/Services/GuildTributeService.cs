using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Models.GuildTribute;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTributeService
    {
        private readonly IGithubApiAccessor _githubApi;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildTributeRepository _guildTributeRepository;
        private readonly IStudentProjectRepository _studentProjectRepository;

        public GuildTributeService(IGithubApiAccessor githubApi, IStudentProjectRepository studentProjectRepository, IStudentRepository studentRepository, IGuildRepository guildRepository, IGuildTributeRepository guildTributeRepository)
        {
            _githubApi = githubApi;
            _studentProjectRepository = studentProjectRepository;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildTributeRepository = guildTributeRepository;
        }

        public TributeInfoResponse[] GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _guildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public TributeInfoResponse[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _guildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _guildTributeRepository
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TributeInfoResponse> CreateTribute(AuthorizedUser user, CreateProjectRequestDto createProject)
        {
            StudentEntity student = await _studentRepository.GetAsync(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.TributeEx.TributeCanBeSendFromStudentAccount(student.Id, createProject.Owner);

            GithubRepositoryInfoDto githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = await _studentProjectRepository.GetOrCreateAsync(githubProject, student);
            GuildEntity guild = _guildRepository.ReadForStudent(student.Id);
            TributeEntity[] allTributes = _guildTributeRepository.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            return _guildTributeRepository.Create(guild, projectEntity).To(TributeInfoResponse.Wrap);
        }

        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            TributeEntity tribute = await _guildTributeRepository.GetAsync(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute.ProjectId);

            if (tribute.ProjectEntity.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                await student.EnsureIsMentor(_guildRepository, tribute.GuildId);
                tribute.SetCanceled();
            }

            TributeEntity updatedTribute = await _guildTributeRepository.UpdateAsync(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }

        public async Task<TributeInfoResponse> CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            TributeEntity tribute = await _guildTributeRepository.GetAsync(tributeCompleteRequest.TributeId);
            GuildMentorUser mentor = await student.EnsureIsMentor(_guildRepository, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute.ProjectId);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteRequest.DifficultLevel, tributeCompleteRequest.Mark);
            TributeEntity updatedTribute = await _guildTributeRepository.UpdateAsync(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }
    }
}