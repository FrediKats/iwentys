using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class QuestGenerator : IEntityGenerator
    {
        private const int QuestCount = 10;
        
        public List<Quest> Quest { get; }
        public List<QuestResponse> QuestResponse { get; } = new List<QuestResponse>();

        public QuestGenerator(List<Student> students)
        {
            Student author = students.First();

            Quest = new QuestFaker(author.Id).Generate(QuestCount);

            foreach (Quest quest in Quest)
            {
                foreach (Student student in students.Take(5))
                {
                    QuestResponse.Add(new QuestResponse()
                    {
                        QuestId = quest.Id,
                        StudentId = student.Id,
                        ResponseTime = DateTime.UtcNow.AddDays(1),
                        Description = new Faker().Lorem.Paragraph()
                    });
                }
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quest>().HasData(Quest);
            modelBuilder.Entity<QuestResponse>().HasData(QuestResponse);
        }
    }
}