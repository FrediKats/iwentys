using System;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TournamentType Type { get; set; }
        public int AuthorId { get; set; }
        public virtual StudentEntity Author { get; set; }

        public static TournamentEntity Create(SystemAdminUser author, CreateTournamentArguments arguments, TournamentType type)
        {
            return new TournamentEntity
            {
                Name = arguments.Name,
                Description = arguments.Description,
                StartTime = arguments.StartTime,
                EndTime = arguments.EndTime,
                Type = type,
                AuthorId = author.Student.Id
            };
        }
    }
}