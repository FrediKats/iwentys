using Iwentys.EntityManager.Common;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.PublicTypes;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataSeeding;

public class StudyEntitiesGenerator : IEntityGenerator
{
    private const int TeacherId = 228617;

    private const int TeacherCount = 20;
    private const int SubjectCount = 8;
    private const StudySemester CurrentSemester = StudySemester.Y20H1;

    public List<StudyCourse> StudyCourses { get; set; }
    public List<StudyProgram> StudyPrograms { get; set; }
    public List<Subject> Subjects { get; set; }
    public List<GroupSubject> GroupSubjects { get; set; }
    public List<StudyGroup> StudyGroups { get; set; }
    public List<UniversitySystemUser> Teachers { get; set; }
    public List<GroupSubjectTeacher> GroupSubjectTeachers { get; set; }
    public StudyEntitiesGenerator()
    {
        Teachers = UsersFaker.Instance.UniversitySystemUsers
            .Generate(TeacherCount)
            .ToList();

        Subjects = SubjectFaker.Instance.Generate(SubjectCount);
        StudyPrograms = new List<StudyProgram> {new StudyProgram {Id = 1, Name = "ИС"}};
        StudyCourses = new List<StudyCourse>
        {
            Create.IsCourse(StudentGraduationYear.Y20),
            Create.IsCourse(StudentGraduationYear.Y21),
            Create.IsCourse(StudentGraduationYear.Y22),
            Create.IsCourse(StudentGraduationYear.Y23),
            Create.IsCourse(StudentGraduationYear.Y24)
        };

        StudyGroups = new List<StudyGroup>();
        foreach (StudyCourse course in StudyCourses)
            StudyGroups.AddRange(CourseGroup(course.Id, (int)course.GraduationYear, 12));

        GroupSubjects = new List<GroupSubject>();
        GroupSubjectTeachers = new List<GroupSubjectTeacher>();
        foreach (Subject subject in Subjects)
        foreach (StudyGroup studyGroup in StudyGroups)
        {
            GroupSubjects.Add(new GroupSubject
            {
                Id = Create.GroupSubjectIdentifierGenerator.Next(),
                SubjectId = subject.Id,
                StudyGroupId = studyGroup.Id,
                StudySemester = CurrentSemester
            });

            GroupSubjectTeachers.Add(new GroupSubjectTeacher()
            {
                TeacherId = TeacherId,
                TeacherType = TeacherType.Lecturer,
                GroupSubjectId = GroupSubjects.Last().Id
            });

            GroupSubjectTeachers.Add(new GroupSubjectTeacher()
            {
                TeacherId = TeacherId,
                GroupSubjectId = GroupSubjects.Last().Id
            });
        }
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyProgram>().HasData(StudyPrograms);
        modelBuilder.Entity<StudyCourse>().HasData(StudyCourses);
        modelBuilder.Entity<StudyGroup>().HasData(StudyGroups);
        modelBuilder.Entity<UniversitySystemUser>().HasData(Teachers);
        modelBuilder.Entity<Subject>().HasData(Subjects);
        modelBuilder.Entity<GroupSubject>().HasData(GroupSubjects);
        modelBuilder.Entity<GroupSubjectTeacher>().HasData(GroupSubjectTeachers);
    }

    public static List<StudyGroup> CourseGroup(int courseId, int course, int groupCount)
    {
        return Enumerable
            .Range(0, groupCount)
            .Select(g => new StudyGroup
            {
                Id = Create.StudyGroupIdentifierGenerator.Next(),
                StudyCourseId = courseId,
                GroupName = new GroupName(course, g).Name
            })
            .ToList();
    }

    public static class Create
    {
        public static readonly IdentifierGenerator StudyGroupIdentifierGenerator = new IdentifierGenerator();
        public static readonly IdentifierGenerator CourseIdentifierGenerator = new IdentifierGenerator();
        public static readonly IdentifierGenerator GroupSubjectIdentifierGenerator = new IdentifierGenerator();

        public static StudyCourse IsCourse(StudentGraduationYear year)
        {
            return new StudyCourse
            {
                Id = CourseIdentifierGenerator.Next(),
                GraduationYear = year,
                StudyProgramId = 1
            };
        }
    }
}