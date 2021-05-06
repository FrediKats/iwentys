using System;
using System.Linq.Expressions;

namespace Iwentys.Domain.Study.Models
{
    public class StudyCourseInfoDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public static Expression<Func<StudyCourse, StudyCourseInfoDto>> FromEntity =>
            entity => new StudyCourseInfoDto
            {
                CourseId = entity.Id,
                CourseTitle = entity.StudyProgram.Name + " " + entity.GraduationYear
            };
    }
}