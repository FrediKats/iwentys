using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Gamification.Entities
{
    public class CourseLeaderboardRow
    {
        public int Position { get; set; }
        public int? OldPosition { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public static List<CourseLeaderboardRow> Create(int courseId, List<SubjectActivity> rows, List<CourseLeaderboardRow> oldRows)
        {
            Dictionary<int, int> mapToOld = oldRows.ToDictionary(v => v.StudentId, v => v.Position);

            return rows
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Take(50)
                .OrderByDescending(r => r.Activity)
                .Select((r, position) => CreateRow(r, courseId, position, mapToOld))
                .ToList();
        }

        private static CourseLeaderboardRow CreateRow(StudyLeaderboardRowDto row, int courseId, int position, Dictionary<int, int> mapToOld)
        {
            int? oldPosition = null;
            if (mapToOld.TryGetValue(row.Student.Id, out var value))
                oldPosition = value;

            return new CourseLeaderboardRow {Position = position + 1, CourseId = courseId, StudentId = row.Student.Id, OldPosition = oldPosition};
        }
    }
}