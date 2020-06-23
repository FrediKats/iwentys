using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iwentys.Models.Types;

namespace Iwentys.Database.Entities
{
    public class GuildProfile
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        
        public GuildHiringPolicy HiringPolicy { get; set; }

        public List<GuildMember> Members { get; set; }
    }
}