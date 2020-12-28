using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class TournamentControllerClient
    {
        public TournamentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<TournamentInfoResponse>> Get()
        {
            return Client.GetFromJsonAsync<List<TournamentInfoResponse>>($"/api/tournaments");
        }

        public Task<TournamentInfoResponse> Get(int tournamentId)
        {
            return Client.GetFromJsonAsync<TournamentInfoResponse>($"/api/tournaments/{tournamentId}");
        }

        public async Task<TournamentInfoResponse> CreateCodeMarathon(CreateCodeMarathonTournamentArguments arguments)
        {
            var result = await Client.PostAsJsonAsync($"/api/tournaments/code-marathon", arguments);
            return await result.Content.ReadFromJsonAsync<TournamentInfoResponse>();
        }
    }
}