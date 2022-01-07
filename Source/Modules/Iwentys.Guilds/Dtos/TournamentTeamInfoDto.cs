using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Guilds;

namespace Iwentys.Modules.Guilds.Dtos
{
    public class TournamentTeamInfoDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public DateTime RegistrationTime { get; set; }
        public int Points { get; init; }

        public List<int> MemberIds { get; set; }

        public static TournamentTeamInfoDto Create(TournamentParticipantTeam team)
        {
            return new TournamentTeamInfoDto
            {
                Id = team.Id,
                TeamName = team.Guild.Title,
                RegistrationTime = team.RegistrationTime,
                MemberIds = team.Members.Select(m => m.MemberId).ToList(),
                Points = team.Members.Sum(m => m.Points)
            };
        }
    }
}