using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Models;

using System;
using System.Collections.Generic;
using Iwentys.Common.Tools;

namespace Iwentys.Features.Quests.Models
{
    public record QuestInfoDto
    {
        public QuestInfoDto(QuestEntity questEntity)
            :  this(
                questEntity.Id,
                questEntity.Title,
                questEntity.Description,
                questEntity.Price,
                questEntity.CreationTime,
                questEntity.Deadline,
                questEntity.State,
                questEntity.IsOutdated,
                new StudentInfoDto(questEntity.Author),
                //TODO: fix this. NRE coz lazy load do not work. https://github.com/kysect/iwentys/issues/138
                questEntity.Responses?.SelectToList(qr => new QuestResponseInfoDto(qr)))
        {
        }

        public QuestInfoDto(int id, string title, string description, int price, DateTime creationTime, DateTime? deadline, QuestState state, bool isOutdated, StudentInfoDto author,
            List<QuestResponseInfoDto> responseInfos)
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
            ResponseInfos = responseInfos;
        }

        public QuestInfoDto()
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
        public List<QuestResponseInfoDto> ResponseInfos { get; set; }

        public static QuestInfoDto Wrap(QuestEntity questEntity)
        {
            return new QuestInfoDto(questEntity);
        }
    }
}