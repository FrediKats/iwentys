using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentJournalPage
    {
        private List<TournamentInfoResponse> _tournaments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tournaments = await ClientHolder.Tournament.Get();
        }
    }
}
