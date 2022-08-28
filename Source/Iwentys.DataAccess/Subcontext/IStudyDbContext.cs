using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public static class StudyDbContextExtensions
{
    public static void OnStudyModelCreating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubjectActivity>().HasKey(s => new { s.SubjectId, s.StudentId });
        modelBuilder.Entity<GroupActivityTable>().HasKey(s => new { s.GroupId, s.SubjectId });
    }
}