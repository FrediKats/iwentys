using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding;

public class QuestGenerator : IEntityGenerator
{
    private const int QuestCount = 10;
    private const int QuestResponseCount = 5;

    public QuestGenerator(List<Student> students)
    {
        Student author = students.First();

        Quest = QuestFaker.Instance.CreateQuestFaker(author.Id).Generate(QuestCount);

        foreach (Quest quest in Quest)
        {
            foreach (Student student in students.Take(QuestResponseCount))
            {
                QuestResponse.Add(QuestFaker.Instance.CreateQuestResponse(quest.Id, student.Id));
            }
        }
    }

    public List<Quest> Quest { get; }
    public List<QuestResponse> QuestResponse { get; } = new List<QuestResponse>();

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quest>().HasData(Quest);
        modelBuilder.Entity<QuestResponse>().HasData(QuestResponse);
    }
}