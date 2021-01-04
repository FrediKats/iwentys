using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;

        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildMemberService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
        }

        public async Task<GuildProfileDto> EnterGuild(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetById(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            IwentysUser profile = await _userRepository.GetById(user.Id);
            var guildMemberEntity = new GuildMember(guild.Profile, profile, GuildMemberType.Member);
            await _guildMemberRepository.InsertAsync(guildMemberEntity);
            await _unitOfWork.CommitAsync();

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuild(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetById(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            IwentysUser profile = await _userRepository.GetById(user.Id);
            var guildMemberEntity = new GuildMember(guild.Profile, profile, GuildMemberType.Requested);
            await _guildMemberRepository.InsertAsync(guildMemberEntity);
            await _unitOfWork.CommitAsync();

            return await Get(guildId, user.Id);
        }

        public async Task LeaveGuild(AuthorizedUser user, int guildId)
        {
            Guild studentGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, guildId);

            //TODO: do smth?
            //TributeEntity userTribute = _guildTributeRepository.Get()
            //    .Where(t => t.GuildId == guildId)
            //    .Where(t => t.ProjectEntity.AuthorId == user.Id)
            //    .SingleOrDefault(t => t.State == TributeState.Active);
            
            //if (userTribute is not null)
            //    await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            await RemoveMember(guildId, user.Id);
        }

        public async Task<GuildMember[]> GetGuildRequests(AuthorizedUser user, int guildId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);

            student.EnsureIsGuildMentor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMember[]> GetGuildBlocked(AuthorizedUser user, int guildId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);

            student.EnsureIsGuildMentor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetById(guildId));
            GuildMember memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);
            memberToKick.MarkBlocked();
            _guildMemberRepository.Update(memberToKick);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnblockStudent(AuthorizedUser user, int guildId, int studentId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! AuthorId: {studentId} GuildId: {guildId}");

            await RemoveMember(guildId, studentId);
        }

        public async Task KickGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetById(guildId));
            GuildMember memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow;
            await RemoveMember(guildId, memberId);
        }

        public async Task AcceptRequest(AuthorizedUser user, int guildId, int memberForAccepting)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMember member = guild.Members.Find(m => m.MemberId == memberForAccepting) ?? throw EntityNotFoundException.Create(typeof(GuildMember), memberForAccepting);

            GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);
            member.Approve(guildMentor);

            _guildMemberRepository.Update(member);
            await _unitOfWork.CommitAsync();
        }

        public async Task RejectRequest(AuthorizedUser user, int guildId, int studentId)
        {
            IwentysUser initiator = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMentor guildMentor = initiator.EnsureIsGuildMentor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.GuildExceptions.RequestWasNotFound(studentId, guildId);

            await RemoveMember(guildId, studentId);
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> Get(int id, int? userId)
        {
            Guild guild = await _guildRepository.GetById(id);
            return await CreateDomain(guild).ToExtendedGuildProfileDto(userId);
        }

        public async Task PromoteToMentor(AuthorizedUser creator, int userForPromotion)
        {
            IwentysUser studentCreator = await _userRepository.GetById(creator.Id);
            var guildMemberEntity = _guildMemberRepository.GetStudentMembership(creator.Id);
            var guildCreator = await studentCreator.EnsureIsCreator(_guildRepository, guildMemberEntity.GuildId);
            
            //TODO: check member state
            var studentMembership = _guildMemberRepository.GetStudentMembership(userForPromotion);
            studentMembership.MakeMentor(guildCreator);

            _guildMemberRepository.Update(studentMembership);
            await _unitOfWork.CommitAsync();
        }

        private GuildDomain CreateDomain(Guild guild)
        {
            return new GuildDomain(guild, _githubIntegrationService, _userRepository, _guildMemberRepository);
        }

        private async Task RemoveMember(int guildId, int userId)
        {
            GuildMember guildMember = _guildMemberRepository.Get().Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.CreatorCannotLeave(userId, guildId);

            _guildMemberRepository.Delete(guildMember);
            await _unitOfWork.CommitAsync();
        }
    }
}