using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfilePreviewDto : GuildProfileShortInfoDto, IResultFormat
    {
        public StudentPartialProfileDto Leader { get; set; }
        public Int32 Rating { get; set; }

        public GuildProfilePreviewDto() : base()
        {
        }

        public GuildProfilePreviewDto(Guild guild) : base(guild)
        {
        }

        public string Format()
        {
            return $"{Title} [{Rating}]";
        }
    }
}