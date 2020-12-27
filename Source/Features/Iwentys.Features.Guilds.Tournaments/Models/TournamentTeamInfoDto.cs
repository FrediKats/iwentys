using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Guilds.Tournaments.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Models
{
    public class TournamentTeamInfoDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public DateTime RegistrationTime { get; set; }

        public List<int> MemberIds { get; set; }

        public static TournamentTeamInfoDto Create(TournamentParticipantTeamEntity team)
        {
            return new TournamentTeamInfoDto()
            {
                Id = team.Id,
                TeamName = team.Guild.Title,
                RegistrationTime = team.RegistrationTime,
                MemberIds = team.Members.Select(m => m.MemberId).ToList()
            };
        }
    }
}