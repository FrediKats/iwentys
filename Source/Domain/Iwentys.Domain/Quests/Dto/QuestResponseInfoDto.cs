using System;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Domain.Quests.Dto
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