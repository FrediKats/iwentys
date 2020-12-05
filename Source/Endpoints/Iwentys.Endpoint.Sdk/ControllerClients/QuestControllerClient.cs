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

        public Task<List<QuestInfoResponse>> GetCreatedByUser()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quest/created");
        }

        public Task<List<QuestInfoResponse>> GetCompletedByUser()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quest/completed");
        }

        public Task<List<QuestInfoResponse>> GetActive()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quest/active");
        }

        public Task<List<QuestInfoResponse>> GetArchived()
        {
            return Client.GetFromJsonAsync<List<QuestInfoResponse>>("/api/quest/archived");
        }

        public async Task<QuestInfoResponse> Create(CreateQuestRequest createQuest)
        {
            HttpResponseMessage result = await Client.PostAsJsonAsync("/api/quest/archived", createQuest);
            return await result.Content.ReadFromJsonAsync<QuestInfoResponse>();
        }

        //[HttpPut("{questId}/send-response")]
        //public async Task<ActionResult<QuestInfoResponse>> SendResponse(int questId)
        //{
        //    AuthorizedUser user = this.TryAuthWithToken();
        //    QuestInfoResponse quest = await _questService.SendResponseAsync(user, questId);
        //    return Ok(quest);
        //}

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