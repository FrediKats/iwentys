using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Features.GithubIntegration.GithubIntegration;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildMemberService
    {
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildLastLeave> _guildLastLeaveRepository;
        private readonly IGenericRepository<IwentysUser> _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly GuildService _guildService;

        public GuildMemberService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork, GuildService guildService)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _guildService = guildService;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildLastLeaveRepository = _unitOfWork.GetRepository<GuildLastLeave>();
        }

        public async Task<GuildProfileDto> EnterGuild(AuthorizedUser authorizedUser, int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser user = await _userRepository.GetById(authorizedUser.Id);
            GuildLastLeave lastLeave = await _guildLastLeaveRepository.FindByIdAsync(user.Id);
            GuildMember guildMember = GetStudentRequest(_guildMemberRepository, authorizedUser.Id);

            GuildMember newMembership = guild.EnterGuild(user, guildMember, lastLeave);

            _guildMemberRepository.Insert(newMembership);
            await _unitOfWork.CommitAsync();
            return await _guildService.Get(guildId);
        }

        public async Task<GuildProfileDto> RequestGuild(AuthorizedUser authorizedUser, int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser user = await _userRepository.GetById(authorizedUser.Id);
            GuildLastLeave lastLeave = await _guildLastLeaveRepository.FindByIdAsync(user.Id);
            GuildMember guildMember = GetStudentRequest(_guildMemberRepository, authorizedUser.Id);

            GuildMember newMembership = guild.RequestEnterGuild(user, guildMember, lastLeave);

            _guildMemberRepository.Insert(newMembership);
            await _unitOfWork.CommitAsync();
            return await _guildService.Get(guildId);
        }

        public async Task LeaveGuild(AuthorizedUser user, int guildId)
        {
            IwentysUser iwentysUser = await _userRepository.GetById(user.Id);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(iwentysUser, _guildLastLeaveRepository);

            Guild studentGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, guildId);

            //TributeEntity userTribute = _guildTributeRepository.Get()
            //    .Where(t => t.GuildId == guildId)
            //    .Where(t => t.ProjectEntity.AuthorId == user.Id)
            //    .SingleOrDefault(t => t.State == TributeState.Active);

            //if (userTribute is not null)
            //    await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            await RemoveMember(guildId, iwentysUser, guildLastLeave);
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
            IwentysUser editorStudentAccount = await _userRepository.GetById(user.Id);
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetById(guildId));
            GuildMember memberToKick = guildDomain.EnsureMemberCanRestrictPermissionForOther(editorStudentAccount, memberId);
            IwentysUser iwentysUser = await _userRepository.GetById(user.Id);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(iwentysUser, _guildLastLeaveRepository);
            
            memberToKick.MarkBlocked(guildLastLeave);
            
            _guildMemberRepository.Update(memberToKick);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnblockStudent(AuthorizedUser user, int guildId, int studentId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);
            IwentysUser iwentysUser = await _userRepository.GetById(studentId);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(iwentysUser, _guildLastLeaveRepository);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! AuthorId: {studentId} GuildId: {guildId}");

            await RemoveMember(guildId, iwentysUser, guildLastLeave);
        }

        public async Task KickGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            IwentysUser editorStudentAccount = await _userRepository.GetById(user.Id);
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetById(guildId));
            GuildMember memberToKick = guildDomain.EnsureMemberCanRestrictPermissionForOther(editorStudentAccount, memberId);
            IwentysUser iwentysUser = await _userRepository.GetById(memberId);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(iwentysUser, _guildLastLeaveRepository);

            await RemoveMember(guildId, iwentysUser, guildLastLeave);
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
            IwentysUser iwentysUser = await _userRepository.GetById(studentId);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(iwentysUser, _guildLastLeaveRepository);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.GuildExceptions.RequestWasNotFound(studentId, guildId);

            await RemoveMember(guildId, iwentysUser, guildLastLeave);
        }
        
        public async Task<UserMembershipState> GetUserMembership(AuthorizedUser creator, int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser user1 = await _userRepository.GetById(creator.Id);
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(user1, _guildLastLeaveRepository);
            GuildMember guildMember = GetStudentRequest(_guildMemberRepository, creator.Id);

            return guild.GetUserMembershipState(user1, guildMember, guildLastLeave);
        }

        public async Task PromoteToMentor(AuthorizedUser creator, int userForPromotion)
        {
            IwentysUser studentCreator = await _userRepository.GetById(creator.Id);
            GuildMember guildMemberEntity = GetStudentMembership(_guildMemberRepository, creator.Id);
            GuildCreator guildCreator = await studentCreator.EnsureIsCreator(_guildRepository, guildMemberEntity.GuildId);

            GuildMember studentMembership = GetStudentMembership(_guildMemberRepository, userForPromotion);
            studentMembership.MakeMentor(guildCreator);

            _guildMemberRepository.Update(studentMembership);
            await _unitOfWork.CommitAsync();
        }

        private GuildDomain CreateDomain(Guild guild)
        {
            return new GuildDomain(guild, _userRepository);
        }

        private async Task RemoveMember(int guildId, IwentysUser user, GuildLastLeave guildLastLeave)
        {
            GuildMember guildMember = _guildMemberRepository.Get().Single(gm => gm.GuildId == guildId && gm.MemberId == user.Id);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.CreatorCannotLeave(user.Id, guildId);

            guildLastLeave.UpdateLeave();
            _guildMemberRepository.Delete(guildMember);
            await _unitOfWork.CommitAsync();
        }

        public static GuildMember GetStudentRequest(IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(m => m.Member.Id == studentId)
                .FirstOrDefault(m => m.MemberType == GuildMemberType.Requested);
        }

        public static GuildMember GetStudentMembership(IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(GuildMember.IsMember())
                .SingleOrDefault(gm => gm.MemberId == studentId);
        }
    }
}