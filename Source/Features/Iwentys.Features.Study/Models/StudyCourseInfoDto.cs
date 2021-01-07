using System;
using System.Linq.Expressions;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
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