using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class TournamentInfoComponent
    {
        private string LinkToTournamentProfile(TournamentInfoResponse tournamentInfo) => $"/tournaments/profile/{tournamentInfo.Id}";
    }
}
