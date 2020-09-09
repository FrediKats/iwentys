using System;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfilePreviewDto : IResultFormat
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }

        public StudentPartialProfileDto Leader { get; set; }
        public Int32 Rating { get; set; }


        public string Format()
        {
            return $"{Title} [{Rating}]";
        }
    }
}