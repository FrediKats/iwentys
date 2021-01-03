using System;

namespace Iwentys.Features.AccountManagement.Entities
{
    public class IwentysUser : UniversitySystemUser
    {
        public bool IsAdmin { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; init; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }


        //TODO: REMOVE ASAP
        public DateTime GuildLeftTime { get; set; }

    }
}