using Iwentys.Domain.Assignments;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IAssignmentsDbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
    }

    public static class AssignmentDbContextExtensions
    {
        public static void OnAssignmentsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentAssignment>().HasKey(a => new { a.AssignmentId, a.StudentId });
        }
    }
}