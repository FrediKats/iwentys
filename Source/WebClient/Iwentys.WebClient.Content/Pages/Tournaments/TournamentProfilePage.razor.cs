using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.WebClient.Content.Pages.Tournaments
{
    public partial class TournamentProfilePage
    {
        private TournamentInfoResponse _tournament;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tournament = await TournamentClient.GetByIdAsync(TournamentId);
        }

        private async Task RegisterToTournament()
        {
            await TournamentClient.RegisterToTournamentAsync(_tournament.Id);
            _tournament = await TournamentClient.GetByIdAsync(TournamentId);
            //TODO: notification about successful registration
        }

        private async Task ForceUpdate()
        {
            _logger.LogWarning($"Force tournament update. Id: {_tournament.Id}");
            await TournamentClient.ForceUpdateAsync(_tournament.Id);
            _tournament = await TournamentClient.GetByIdAsync(TournamentId);
        }
    }
}
