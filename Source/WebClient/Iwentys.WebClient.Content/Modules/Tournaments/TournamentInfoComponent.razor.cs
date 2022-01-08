using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class TournamentInfoComponent
    {
        private string LinkToTournamentProfile(TournamentInfoResponse tournamentInfo) => $"/tournaments/profile/{tournamentInfo.Id}";
    }
}
