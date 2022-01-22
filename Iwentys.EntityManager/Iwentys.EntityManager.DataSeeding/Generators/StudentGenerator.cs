using Bogus;
using Iwentys.EntityManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataSeeding;

public class StudentGenerator : IEntityGenerator
{
    private const int StudentCount = 200;

    public List<Student> Students { get; set; }

    public StudentGenerator(List<StudyGroup> studyGroups)
    {
        Students = UsersFaker.Instance.Students
            .Generate(StudentCount)
            .ToList();

        Students.Add(new Student
        {
            Id = 228617,
            FirstName = "Фреди",
            MiddleName = "Кисикович",
            SecondName = "Катс",
            IsAdmin = true,
            GithubUsername = "FrediKats",
            CreationTime = DateTime.UtcNow,
            LastOnlineTime = DateTime.UtcNow,
            AvatarUrl = new Faker().Image.PicsumUrl(),
            GroupId = studyGroups.First(g => g.GroupName.Contains("3505")).Id
        });
        
        foreach (Student student in Students)
            student.GroupId = FakerSingleton.Instance.PickRandom(studyGroups).Id;

        Students
            .GroupBy(s => s.GroupId)
            .Select(g => g.FirstOrDefault())
            .Where(s => s is not null)
            .ToList()
            .ForEach(s =>
            {
                StudyGroup studyGroup = studyGroups.First();
                studyGroup.GroupAdminId = s.Id;
            });
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(Students);
    }
}