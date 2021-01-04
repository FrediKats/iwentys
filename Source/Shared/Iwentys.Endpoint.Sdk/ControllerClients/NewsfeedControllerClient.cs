using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class NewsfeedControllerClient
    {
        public NewsfeedControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task CreateSubjectNewsfeed(int subjectId, NewsfeedCreateViewModel createViewModel)
        {
            await Client.PostAsJsonAsync($"/api/newsfeed/subject/{subjectId}/newsfeed", createViewModel);
        }

        public async Task CreateGuildNewsfeed(int guildId, NewsfeedCreateViewModel createViewModel)
        {
            await Client.PostAsJsonAsync($"/api/newsfeed/guild/{guildId}/newsfeed", createViewModel);
        }

        public Task<List<NewsfeedViewModel>> GetForSubject(int subjectId)
        {
            return Client.GetFromJsonAsync<List<NewsfeedViewModel>>($"/api/newsfeed/subject/{subjectId}");
        }

        public Task<List<NewsfeedViewModel>> GetForGuild(int guildId)
        {
            return Client.GetFromJsonAsync<List<NewsfeedViewModel>>($"/api/newsfeed/guild/{guildId}");
        }
    }
}