using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl.Http;
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
            var result = await new FlurlClient(Client)
                .Request("/api/tournaments/code-marathon")
                .PostJsonAsync(arguments);
            return await result.GetJsonAsync<TournamentInfoResponse>();
        }

        public async Task RegisterToTournament(int tournamentId)
        {
            await new FlurlClient(Client)
                .Request($"/api/tournaments/{tournamentId}/register")
                .PutAsync();
        }
    }
}