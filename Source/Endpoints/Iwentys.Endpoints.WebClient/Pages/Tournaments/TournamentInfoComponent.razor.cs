using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Tournaments
{
    public partial class TournamentInfoComponent
    {
        private string LinkToTournamentProfile(TournamentInfoResponse tournamentInfo) => $"/tournaments/profile/{tournamentInfo.Id}";
    }
}
