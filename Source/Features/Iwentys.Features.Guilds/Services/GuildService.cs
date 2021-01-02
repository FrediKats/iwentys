using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<GuildPinnedProject> _guildPinnedProjectRepository;

        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildPinnedProjectRepository = _unitOfWork.GetRepository<GuildPinnedProject>();
        }

        public async Task<GuildProfileShortInfoDto> CreateAsync(AuthorizedUser creator, GuildCreateRequestDto arguments)
        {
            Student creatorUser = await _studentRepository.GetByIdAsync(creator.Id);

            Guild userGuild = _guildMemberRepository.ReadForStudent(creatorUser.Id);
            if (userGuild is not null)
                throw new InnerLogicException("Student already in guild");

            var guildEntity = Guild.Create(creatorUser, arguments);
            await _guildRepository.InsertAsync(guildEntity);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guildEntity);
        }

        public async Task<GuildProfileShortInfoDto> UpdateAsync(AuthorizedUser user, GuildUpdateRequestDto arguments)
        {
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            Guild guild = await _guildRepository.GetByIdAsync(arguments.Id);
            var guildMentor = student.EnsureIsGuildMentor(guild);

            guild.Update(arguments);

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMember guildMember in guild.Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.Approve(guildMentor);

            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guild);
        }

        public async Task<GuildProfileShortInfoDto> ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            Guild guild = await _guildRepository.GetByIdAsync(guildId);
            
            var admin = student.EnsureIsAdmin();
            guild.Approve(admin);

            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(await _guildRepository.GetByIdAsync(guildId));
        }

        public List<GuildProfileDto> GetOverview(Int32 skippedCount, Int32 takenCount)
        {
            //TODO: add order
            return _guildRepository
                .Get()
                .Skip(skippedCount)
                .Take(takenCount)
                .Select(GuildProfileDto.FromEntity)
                .ToList();
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> GetAsync(int id, int? userId)
        {
            Guild guild = await _guildRepository.GetByIdAsync(id);

            return await new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildMemberRepository)
                .ToExtendedGuildProfileDto(userId);
        }

        public GuildProfileDto FindStudentGuild(int userId)
        {
            Guild guild = _guildMemberRepository.ReadForStudent(userId);
            if (guild is null)
                return null;

            return new GuildProfileDto(guild);
        }

        public async Task<GithubRepositoryInfoDto> AddPinnedRepositoryAsync(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            Guild guild = await _guildRepository.GetByIdAsync(guildId);
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            var guildMentor = student.EnsureIsGuildMentor(guild);

            GithubRepositoryInfoDto repositoryInfoDto = await _githubIntegrationService.GetRepository(owner, projectName);
            var guildPinnedProjectEntity = GuildPinnedProject.Create(guildId, repositoryInfoDto);
            await _guildPinnedProjectRepository.InsertAsync(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
            
            return repositoryInfoDto;
        }

        public async Task UnpinProject(AuthorizedUser user, int guildId, long pinnedProjectId)
        {
            Guild guild = await _guildRepository.GetByIdAsync(guildId);
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            var guildMentor = student.EnsureIsGuildMentor(guild);

            var guildPinnedProjectEntity = await _guildPinnedProjectRepository.GetByIdAsync(pinnedProjectId);
            _guildPinnedProjectRepository.Delete(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            Guild guild = await _guildRepository.GetByIdAsync(guildId);
            var domain = new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildMemberRepository);
            return await domain.GetMemberDashboard();
        }
    }
}