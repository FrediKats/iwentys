using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentJournalPage
    {
        private GuildControllerClient _guildControllerClient;
        private GuildTributeControllerClient _guildTributeControllerControllerClient;
        private TournamentControllerClient _tournamentControllerClient;

        private List<TournamentInfoResponse> _tournaments;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _guildTributeControllerControllerClient = new GuildTributeControllerClient(httpClient);
            _tournamentControllerClient = new TournamentControllerClient(httpClient);

            _tournaments = await _tournamentControllerClient.Get();
        }

        private string LinkToTournamentProfile(TournamentInfoResponse tournamentInfo) => $"/tournaments/profile/{tournamentInfo.Id}";
    }
}
