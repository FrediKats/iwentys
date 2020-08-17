﻿using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Gamification;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IQuestService
    {
        List<QuestInfoDto> GetCreatedByUser(AuthorizedUser user);
        List<QuestInfoDto> GetCompletedByUser(AuthorizedUser user);
        List<QuestInfoDto> GetActive();
        List<QuestInfoDto> GetArchive();

        QuestInfoDto SendResponse(AuthorizedUser user, int id);

    }
}