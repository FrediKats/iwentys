using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Raids.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Raids.Models
{
    public class RaidProfileDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RaidType RaidType { get; set; }

        public StudentInfoDto Author { get; set; }

        public ICollection<StudentInfoDto> Visitors { get; set; }

        public static Expression<Func<Raid, RaidProfileDto>> FromEntity =>
            entity => new RaidProfileDto()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CreationTime = entity.CreationTime,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                RaidType = entity.RaidType,
                Author = new StudentInfoDto(entity.Author),
                Visitors = entity.Visitors.Select(s => new StudentInfoDto(s.Visitor)).ToList()
            };
    }
}