using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Tournaments
{
    public partial class CreateTournamentPage
    {
        private CreateCodeMarathonTournamentArguments CurrentArguments = new CreateCodeMarathonTournamentArguments();


        private async Task Create()
        {
            await TournamentClient.CreateCodeMarathonAsync(CurrentArguments);
        }
    }
}
