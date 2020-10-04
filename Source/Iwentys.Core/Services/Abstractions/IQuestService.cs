using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IQuestService
    {
        List<QuestInfoResponse> GetCreatedByUser(AuthorizedUser user);
        List<QuestInfoResponse> GetCompletedByUser(AuthorizedUser user);
        List<QuestInfoResponse> GetActive();
        List<QuestInfoResponse> GetArchived();

        QuestInfoResponse Create(AuthorizedUser user, CreateQuestRequest createQuest);

        QuestInfoResponse SendResponse(AuthorizedUser user, int id);
        QuestInfoResponse Complete(AuthorizedUser author, int questId, int userId);
        QuestInfoResponse Revoke(AuthorizedUser author, int questId);
    }
}