using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentProfilePage
    {
        private TournamentInfoResponse _tournament;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tournament = await ClientHolder.ApiTournamentsGetAsync(TournamentId);
        }

        private async Task RegisterToTournament()
        {
            await ClientHolder.ApiTournamentsRegisterAsync(_tournament.Id);
            _tournament = await ClientHolder.ApiTournamentsGetAsync(TournamentId);
            //TODO: notification about successful registration
        }

        private async Task ForceUpdate()
        {
            _logger.LogWarning($"Force tournament update. Id: {_tournament.Id}");
            await ClientHolder.ApiTournamentsForceUpdateAsync(_tournament.Id);
            _tournament = await ClientHolder.ApiTournamentsGetAsync(TournamentId);
        }
    }
}
