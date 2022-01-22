using System;
using Iwentys.AccountManagement;
using Iwentys.Domain.Quests;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;

namespace Iwentys.Gamification;

public class QuestResponseInfoDto
{
    public QuestResponseInfoDto(QuestResponse questResponse) : this()
    {
        Student = EntityManagerApiDtoMapper.Map(questResponse.Student);
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