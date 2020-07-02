using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
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
        private readonly IUserProfileRepository _userProfileRepository;

        public GuildService(IGuildRepository guildRepository, IUserProfileRepository userProfileRepository)
        {
            _guildRepository = guildRepository;
            _userProfileRepository = userProfileRepository;
        }

        public GuildProfileDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments)
        {
            UserProfile creatorUser = _userProfileRepository.Get(creator.Id);

            Guild userGuild = _guildRepository.ReadForUser(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("User already in guild");

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
            _userProfileRepository
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

        public GuildProfileDto GetUserGuild(int userId)
        {
            return _guildRepository.ReadForUser(userId).To(GuildProfileDto.Create);
        }

        public VotingInfoDto StartVotingForLeader(AuthorizedUser user, int guildId, VotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }

        public VotingInfoDto StartVotingForTotem(AuthorizedUser user, int guildId, GuildTotemVotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }
    }
}