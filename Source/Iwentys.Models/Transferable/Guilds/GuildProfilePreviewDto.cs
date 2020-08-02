using System;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfilePreviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }

        public Student Leader { get; set; }
        public Int32 Rating { get; set; }
    }
}