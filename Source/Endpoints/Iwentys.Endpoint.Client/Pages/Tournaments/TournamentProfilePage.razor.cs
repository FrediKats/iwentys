using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentProfilePage
    {
        private TournamentControllerClient _tournamentControllerClient;

        private TournamentInfoResponse _tournament;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _tournamentControllerClient = new TournamentControllerClient(httpClient);

            _tournament = await _tournamentControllerClient.Get(TournamentId);
        }
    }
}
