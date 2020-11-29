using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class NewsfeedControllerClient
    {
        public NewsfeedControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<NewsfeedInfoResponse>> GetForSubject(int subjectId)
        {
            return Client.GetFromJsonAsync<List<NewsfeedInfoResponse>>($"/api/newsfeed/subject/{subjectId}");
        }

        public Task<List<NewsfeedInfoResponse>> GetForGuild(int guildId)
        {
            return Client.GetFromJsonAsync<List<NewsfeedInfoResponse>>($"/api/newsfeed/guild/{guildId}");
        }
    }
}