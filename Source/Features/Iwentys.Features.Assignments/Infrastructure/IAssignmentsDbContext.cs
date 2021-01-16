using Iwentys.Features.Assignments.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Assignments.Infrastructure
{
    public interface IAssignmentsDbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnAssignmentsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentAssignment>().HasKey(a => new { a.AssignmentId, a.StudentId });
        }
    }
}