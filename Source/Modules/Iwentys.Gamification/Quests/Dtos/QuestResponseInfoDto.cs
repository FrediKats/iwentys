using System;
using Iwentys.AccountManagement;
using Iwentys.Domain.Quests;

namespace Iwentys.Gamification
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