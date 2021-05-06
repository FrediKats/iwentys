using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    //TODO: it's some hack. Need to move this in database project
    public interface IStudyDbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<GroupSubject> GroupSubjects { get; set; }
        public DbSet<StudyCourse> StudyCourses { get; set; }
    }

    public static class StudyDbContextExtensions
    {
        public static void OnStudyModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectActivity>().HasKey(s => new { s.GroupSubjectId, s.StudentId });
        }
    }
}