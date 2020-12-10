using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models.GuildTribute;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class GuildTributeControllerClient
    {
        public GuildTributeControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<TributeInfoResponse>> GetGuildTribute(int guildId)
        {
            //TODO: rework it later
            return Client.GetFromJsonAsync<List<TributeInfoResponse>>($"/api/guild/tribute/get-for-guild?guildId={guildId}");
        }
    }
}