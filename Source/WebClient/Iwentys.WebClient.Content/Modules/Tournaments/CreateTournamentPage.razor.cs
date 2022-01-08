using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class CreateTournamentPage
    {
        private CreateCodeMarathonTournamentArguments CurrentArguments = new CreateCodeMarathonTournamentArguments();


        private async Task Create()
        {
            await _tournamentClient.CreateCodeMarathonAsync(CurrentArguments);
        }
    }
}
