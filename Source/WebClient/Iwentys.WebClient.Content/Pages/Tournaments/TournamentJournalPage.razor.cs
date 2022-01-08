using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Tournaments
{
    public partial class TournamentJournalPage
    {
        private ICollection<TournamentInfoResponse> _tournaments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tournaments = await TournamentClient.GetAsync();
        }
    }
}
