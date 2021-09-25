using System;
using Iwentys.Domain.Quests;
using Iwentys.Modules.AccountManagement.Dtos;

namespace Iwentys.Modules.Gamification.Quests.Dtos
{
    public class QuestResponseInfoDto
    {
        public QuestResponseInfoDto(QuestResponse questResponse) : this()
        {
            Student = new IwentysUserInfoDto(questResponse.Student);
            ResponseTime = questResponse.ResponseTime;
            Description = questResponse.Description;
        }

        public QuestResponseInfoDto()
        {
        }

        public IwentysUserInfoDto Student { get; set; }
        public DateTime ResponseTime { get; set; }
        public string Description { get; set; }
    }
}