using System;
using Iwentys.Domain.Gamification;

namespace Iwentys.Domain.Models
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