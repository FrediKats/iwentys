using System;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Quests.Models
{
    public class QuestResponseInfoDto
    {
        public StudentInfoDto Student { get; set; }
        public DateTime ResponseTime { get; set; }

        public QuestResponseInfoDto(QuestResponseEntity questResponse) : this()
        {
            Student = new StudentInfoDto(questResponse.Student);
            ResponseTime = questResponse.ResponseTime;
        }

        public QuestResponseInfoDto()
        {
        }
    }
}