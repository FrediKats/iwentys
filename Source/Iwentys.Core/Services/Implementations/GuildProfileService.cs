using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Tools;
using Iwentys.Database.Entities;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Transferable.Guilds;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildProfileService : IGuildProfileService
    {
        private readonly IGuildProfileRepository _guildProfileRepository;

        public GuildProfileService(IGuildProfileRepository guildProfileRepository)
        {
            _guildProfileRepository = guildProfileRepository;
        }


        public GuildProfileDto Create(int creator, GuildCreateArgumentDto arguments)
        {
            //TODO: check if user already in guild
            var newGuild = new GuildProfile
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title
            };

            return _guildProfileRepository.Create(newGuild).To(GuildProfileDto.Create);
        }

        public GuildProfileDto Update(int creator, GuildUpdateArgumentDto arguments)
        {
            //TODO: check permission
            GuildProfile info = _guildProfileRepository.Get(arguments.Id);
            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;
            return _guildProfileRepository.Update(info).To(GuildProfileDto.Create);
        }

        public GuildProfileDto[] Get()
        {
            return _guildProfileRepository.Read().Select(GuildProfileDto.Create).ToArray();
        }

        public GuildProfileDto Get(int id)
        {
            return _guildProfileRepository.Get(id).To(GuildProfileDto.Create);
        }
    }
}