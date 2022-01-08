using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Tournaments
{
    public partial class TournamentInfoComponent
    {
        private string LinkToTournamentProfile(TournamentInfoResponse tournamentInfo) => $"/tournaments/profile/{tournamentInfo.Id}";
    }
}
