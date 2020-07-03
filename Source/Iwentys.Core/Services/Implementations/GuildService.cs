using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildService : IGuildService
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITributeRepository _tributeRepository;
        private readonly IStudentProjectRepository _studentProjectRepository;

        public GuildService(IGuildRepository guildRepository, IStudentRepository studentRepository, IStudentProjectRepository studentProjectRepository, ITributeRepository tributeRepository)
        {
            _guildRepository = guildRepository;
            _studentRepository = studentRepository;
            _studentProjectRepository = studentProjectRepository;
            _tributeRepository = tributeRepository;
        }

        public GuildProfileDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments)
        {
            Student creatorUser = _studentRepository.Get(creator.Id);

            Guild userGuild = _guildRepository.ReadForStudent(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("Student already in guild");

            var newGuild = new Guild
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMember>
            {
                new GuildMember {Guild = newGuild, Member = creatorUser, MemberType = GuildMemberType.Creator}
            };

            return _guildRepository.Create(newGuild).To(GuildProfileDto.Create);
        }

        public GuildProfileDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments)
        {
            //TODO: check permission
            Guild info = _guildRepository.Get(arguments.Id);
            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;
            return _guildRepository.Update(info).To(GuildProfileDto.Create);
        }

        public GuildProfileDto ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            _studentRepository
                .Get(user.Id)
                .EnsureIsAdmin();

            Guild guild = _guildRepository.Get(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            return _guildRepository.Update(guild).To(GuildProfileDto.Create);
        }

        public GuildProfileDto[] Get()
        {
            return _guildRepository.Read().Select(GuildProfileDto.Create).ToArray();
        }

        public GuildProfileDto Get(int id)
        {
            return _guildRepository.Get(id).To(GuildProfileDto.Create);
        }

        public GuildProfileDto GetStudentGuild(int userId)
        {
            return _guildRepository.ReadForStudent(userId).To(GuildProfileDto.Create);
        }

        public VotingInfoDto StartVotingForLeader(AuthorizedUser user, int guildId, GuildLeaderVotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }

        public VotingInfoDto StartVotingForTotem(AuthorizedUser user, int guildId, GuildTotemVotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }

        public void SendTribute(AuthorizedUser user, int guildId, int projectId)
        {
            Student student = _studentRepository.Get(user.Id);
            Guild guild = _guildRepository.Get(guildId);
            StudentProject project = _studentProjectRepository.Get(projectId);
            
            if (_tributeRepository.Read().Any(t => t.ProjectId == projectId))
                throw new InnerLogicException("Repository already used for tribute");

            //TODO: add check for user in guild, that Totem is exists

            var tribute = new Tribute
            {
                GuildId =  guild.Id,
                TotemId = guild.TotemId,
                ProjectId = project.Id
            };

            _tributeRepository.Create(tribute);
        }
    }
}