using System;
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

        public static Expression<Func<TournamentEntity, TournamentInfoResponse>> FromEntity =>
            tournamentEntity =>
                new TournamentInfoResponse
                {
                    Id = tournamentEntity.Id,
                    Name = tournamentEntity.Name,
                    Description = tournamentEntity.Description,
                    StartTime = tournamentEntity.StartTime,
                    EndTime = tournamentEntity.EndTime,
                    Type = tournamentEntity.Type
                };

        public static TournamentInfoResponse Wrap(TournamentEntity tournamentEntity)
        {
            return new TournamentInfoResponse
            {
                Id = tournamentEntity.Id,
                Name = tournamentEntity.Name,
                Description = tournamentEntity.Description,
                StartTime = tournamentEntity.StartTime,
                EndTime = tournamentEntity.EndTime,
                Type = tournamentEntity.Type
            };
        }
    }
}