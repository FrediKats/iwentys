using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
{
    public class GuildProfilePreviewDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfilePreviewDto()
        {
        }

        public GuildProfilePreviewDto(GuildEntity guild) : base(guild)
        {
        }

        public StudentPartialProfileDto Leader { get; set; }
        public int Rating { get; set; }

        public string Format()
        {
            return $"{Title} [{Rating}]";
        }
    }
}