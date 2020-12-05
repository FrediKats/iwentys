using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildProfilePreviewDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfilePreviewDto()
        {
        }

        public GuildProfilePreviewDto(GuildEntity guild) : base(guild)
        {
        }

        public StudentInfoDto Leader { get; set; }
        public int Rating { get; set; }

        public string Format()
        {
            return $"{Title} [{Rating}]";
        }
    }
}