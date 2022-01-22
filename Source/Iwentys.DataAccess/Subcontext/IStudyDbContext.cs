using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public static class StudyDbContextExtensions
{
    public static void OnStudyModelCreating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubjectActivity>().HasKey(s => new { s.GroupSubjectId, s.StudentId });

        modelBuilder.Entity<GroupSubjectMentor>().HasKey(gsm => new { gsm.UserId, gsm.GroupSubjectId, gsm.IsLector });
    }
}