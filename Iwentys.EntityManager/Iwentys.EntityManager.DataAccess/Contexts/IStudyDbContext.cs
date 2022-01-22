using Iwentys.EntityManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataAccess;

public interface IStudyDbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<StudyGroup> StudyGroups { get; set; }
    public DbSet<StudyProgram> StudyPrograms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<GroupSubject> GroupSubjects { get; set; }
    public DbSet<GroupSubjectTeacher> GroupSubjectMentors { get; set; }
    public DbSet<StudyCourse> StudyCourses { get; set; }
}

public static class StudyDbContextExtensions
{
    public static void OnStudyModelCreating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupSubjectTeacher>().HasKey(gsm => new { UserId = gsm.TeacherId, gsm.GroupSubjectId, gsm.TeacherType });
    }
}