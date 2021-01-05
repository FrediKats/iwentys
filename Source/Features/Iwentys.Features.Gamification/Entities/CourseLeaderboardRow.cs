using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Gamification.Entities
{
    public class CourseLeaderboardRow
    {
        public int Position { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public static List<CourseLeaderboardRow> Create(int courseId, List<SubjectActivity> rows)
        {
            return rows
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Take(50)
                .OrderByDescending(r => r.Activity)
                .Select((r, position) => new CourseLeaderboardRow {Position = position + 1, CourseId = courseId, StudentId = r.Student.Id})
                .ToList();
        }
    }
}