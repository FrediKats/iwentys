using System;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Quests.Models
{
    public record QuestInfoResponse(
        int Id,
        string Title,
        string Description,
        int Price,
        DateTime CreationTime,
        DateTime? Deadline,
        QuestState State,
        bool IsOutdated,
        StudentInfoDto Author)
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
        
        public static QuestInfoResponse Wrap(QuestEntity questEntity)
        {
            return new QuestInfoResponse(questEntity);
        }
    }
}