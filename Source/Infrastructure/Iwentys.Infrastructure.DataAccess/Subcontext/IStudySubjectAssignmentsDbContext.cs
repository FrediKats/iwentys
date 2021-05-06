using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IStudySubjectAssignmentsDbContext
    {
        public DbSet<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
    }
    public static class StudySubjectAssignmentsDbContextExtensions
    {
        public static void OnStudySubjectAssignmentsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupSubjectAssignment>().HasKey(gsa => new { gsa.GroupId, gsa.SubjectAssignmentId });
        }
    }
}