using System;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Quests.Entities;

namespace Iwentys.Features.Quests.Models
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