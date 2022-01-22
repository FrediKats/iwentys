using System.Threading.Tasks;
using Blazor.Extensions.Logging;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class TournamentProfilePage
{
    private TournamentInfoResponse _tournament;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _tournament = await _tournamentClient.GetByIdAsync(TournamentId);
    }

    private async Task RegisterToTournament()
    {
        await _tournamentClient.RegisterToTournamentAsync(_tournament.Id);
        _tournament = await _tournamentClient.GetByIdAsync(TournamentId);
        //TODO: notification about successful registration
    }

    private async Task ForceUpdate()
    {
        _logger.LogWarning($"Force tournament update. Id: {_tournament.Id}");
        await _tournamentClient.ForceUpdateAsync(_tournament.Id);
        _tournament = await _tournamentClient.GetByIdAsync(TournamentId);
    }
}