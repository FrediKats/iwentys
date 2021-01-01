using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Features.Raids.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class RaidControllerClient
    {
        public RaidControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<RaidProfileDto>> Get()
        {
            return new FlurlClient(Client)
                .Request("/api/raids/profile")
                .GetJsonAsync<List<RaidProfileDto>>();
        }

        public Task<RaidProfileDto> Get(int raidId)
        {
            return new FlurlClient(Client)
                .Request($"/api/raids/profile/{raidId}")
                .GetJsonAsync<RaidProfileDto>();
        }

        public Task RegisterOnRaid(int raidId)
        {
            return new FlurlClient(Client)
                .Request($"/api/raids/profile/{raidId}/register")
                .PutAsync();
        }
    }
}