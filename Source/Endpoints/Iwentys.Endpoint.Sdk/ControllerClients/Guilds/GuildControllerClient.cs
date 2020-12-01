using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.ViewModels.Guilds;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class GuildControllerClient
    {
        public GuildControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }


        public Task<List<GuildProfilePreviewDto>> GetOverview(int skip = 0, int take = 20)
        {
            //TODO: rework it later
            return Client.GetFromJsonAsync<List<GuildProfilePreviewDto>>($"/api/guild?skip={skip}&take={take}");
        }

        public Task<GuildProfileDto> Get(int id)
        {
            return Client.GetFromJsonAsync<GuildProfileDto>($"/api/guild/{id}");
        }

        public Task<GuildProfileDto> GetForMember(int memberId)
        {
            //TODO: fix
            return Task.FromResult<GuildProfileDto>(null);
            return Client.GetFromJsonAsync<GuildProfileDto>($"/api/guild/for-member?memberId={memberId}");
        }
    }
}