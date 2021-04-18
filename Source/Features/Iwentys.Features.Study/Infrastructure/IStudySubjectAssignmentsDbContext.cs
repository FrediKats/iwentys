using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Infrastructure
{
    public interface IStudySubjectAssignmentsDbContext
    {
        public DbSet<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
    }
    public static class DbContextExtensions
    {
        public static void OnStudySubjectAssignmentsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupSubjectAssignment>().HasKey(gsa => new { gsa.GroupId, gsa.SubjectAssignmentId });
        }
    }
}