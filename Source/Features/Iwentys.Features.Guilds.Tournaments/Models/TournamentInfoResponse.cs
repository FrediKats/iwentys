using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Enums;

namespace Iwentys.Features.Guilds.Tournaments.Models
{
    public class TournamentInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TournamentType Type { get; set; }

        public IEnumerable<TournamentTeamInfoDto> Teams { get; set; }

        public static Expression<Func<Tournament, TournamentInfoResponse>> FromEntity =>
            tournamentEntity =>
                new TournamentInfoResponse
                {
                    Id = tournamentEntity.Id,
                    Name = tournamentEntity.Name,
                    Description = tournamentEntity.Description,
                    StartTime = tournamentEntity.StartTime,
                    EndTime = tournamentEntity.EndTime,
                    Type = tournamentEntity.Type,
                    //FYI: do not replace, it wouldn't compile to SQL
                    Teams = tournamentEntity.Teams.Select(t => TournamentTeamInfoDto.Create(t))
                };

        //FYI: HACK
        public TournamentInfoResponse OrderByRate()
        {
            Teams = Teams.OrderByDescending(t => t.Points).ToList();
            return this;
        }
    }
}