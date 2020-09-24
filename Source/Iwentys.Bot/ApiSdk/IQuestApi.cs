using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Transferable.Gamification;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IQuestApi
    {
        [Get("/api/Quest/created")]
        Task<List<QuestInfoDto>> GetCreatedByUser();

        [Get("/api/Quest/completed")]
        Task<List<QuestInfoDto>> GetCompletedByUser();

        [Get("/api/Quest/active")]
        Task<List<QuestInfoDto>> GetActive();

        [Post("/api/Quest/archived")]
        Task<QuestInfoDto> Create([Body] CreateQuestDto createQuest);

        [Put("/api/Quest/{questId}/send-response")]
        Task<List<QuestInfoDto>> SendResponse(int questId);

        [Put("/api/Quest/{questId}/complete")]
        Task<List<QuestInfoDto>> Complete(int questId, int userId);

        [Put("/api/Quest/{questId}/revoke")]
        Task<List<QuestInfoDto>> Revoke(int questId);
    }
}