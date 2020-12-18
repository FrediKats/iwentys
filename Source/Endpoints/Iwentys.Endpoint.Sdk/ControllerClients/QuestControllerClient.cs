using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class QuestControllerClient
    {
        public QuestControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<QuestInfoResponse> Get(int questId)
        {
            return Client.GetFromJsonAsync<QuestInfoResponse>($"/api/quests/{questId}");
        }

        public Task<List<QuestInfoResponse>> GetCreatedByUser()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quests/created");
        }

        public Task<List<QuestInfoResponse>> GetCompletedByUser()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quests/completed");
        }

        public Task<List<QuestInfoResponse>> GetActive()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quests/active");
        }

        public Task<List<QuestInfoResponse>> GetArchived()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quests/archived");
        }

        public async Task Create(CreateQuestRequest createQuest)
        {
            await Client.PostAsJsonAsync("/api/quests", createQuest);
        }

        public async Task<QuestInfoResponse> SendResponse(int questId)
        {
            return await Client.GetFromJsonAsync<QuestInfoResponse>($"/api/quests/{questId}");
        }
        
        //[HttpPut("{questId}/complete")]
        //public async Task<ActionResult<QuestInfoResponse>> Complete([FromRoute] int questId, [FromQuery] int userId)
        //{
        //    AuthorizedUser author = this.TryAuthWithToken();
        //    QuestInfoResponse quest = await _questService.CompleteAsync(author, questId, userId);
        //    return Ok(quest);
        //}

        //[HttpPut("{questId}/revoke")]
        //public async Task<ActionResult<QuestInfoResponse>> Revoke([FromRoute] int questId)
        //{
        //    AuthorizedUser author = this.TryAuthWithToken();
        //    QuestInfoResponse quest = await _questService.RevokeAsync(author, questId);
        //    return Ok(quest);
        //}
    }
}