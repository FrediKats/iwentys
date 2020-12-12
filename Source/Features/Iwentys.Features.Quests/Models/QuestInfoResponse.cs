using System;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Quests.Models
{
    public record QuestInfoResponse
    {
        public QuestInfoResponse(QuestEntity questEntity)
            : this(
                questEntity.Id,
                questEntity.Title,
                questEntity.Description,
                questEntity.Price,
                questEntity.CreationTime,
                questEntity.Deadline,
                questEntity.State,
                questEntity.IsOutdated,
                new StudentInfoDto(questEntity.Author))
        {
        }

        public QuestInfoResponse(int id, string title, string description, int price, DateTime creationTime, DateTime? deadline, QuestState state, bool isOutdated, StudentInfoDto author)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            CreationTime = creationTime;
            Deadline = deadline;
            State = state;
            IsOutdated = isOutdated;
            Author = author;
        }

        public QuestInfoResponse()
        {
        }
        
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int Price { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime? Deadline { get; init; }
        public QuestState State { get; init; }
        public bool IsOutdated { get; init; }
        public StudentInfoDto Author { get; init; }

        public static QuestInfoResponse Wrap(QuestEntity questEntity)
        {
            return new QuestInfoResponse(questEntity);
        }
    }
}