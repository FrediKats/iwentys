using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentProfilePage
    {
        private TournamentInfoResponse _tournament;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tournament = await ClientHolder.Tournament.Get(TournamentId);
        }

        private async Task RegisterToTournament()
        {
            await ClientHolder.Tournament.RegisterToTournament(_tournament.Id);
            _tournament = await ClientHolder.Tournament.Get(TournamentId);
        }

        private async Task ForceUpdate()
        {
            await ClientHolder.Tournament.ForceUpdate(_tournament.Id);
            _tournament = await ClientHolder.Tournament.Get(TournamentId);
        }
    }
}
