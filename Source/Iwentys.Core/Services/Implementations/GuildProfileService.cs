using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Entities;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildProfileService : IGuildProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IGuildProfileRepository _guildProfileRepository;

        public GuildProfileService(IUserProfileRepository userProfileRepository, IGuildProfileRepository guildProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _guildProfileRepository = guildProfileRepository;
        }


        public GuildProfile Create(int creator, GuildCreateArgumentDto arguments)
        {
            //TODO: check if user already in guild
            var newGuild = new GuildProfile
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title
            };

            return _guildProfileRepository.Create(newGuild);
        }

        public GuildProfile Update(int creator, GuildUpdateArgumentDto arguments)
        {
            //TODO: check permission
            GuildProfile info = _guildProfileRepository.Get(arguments.Id);
            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;
            return _guildProfileRepository.Update(info);
        }

        public GuildProfile[] Get()
        {
            return _guildProfileRepository.Read();
        }

        public GuildProfile Get(int id)
        {
            return _guildProfileRepository.Get(id);
        }
    }
}