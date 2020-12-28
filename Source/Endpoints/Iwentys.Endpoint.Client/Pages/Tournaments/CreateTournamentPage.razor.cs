using System;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class CreateTournamentPage
    {
        private TournamentControllerClient _tournamentControllerClient;

        private string _name;
        private string _description;
        private DateTime? _startTime;
        private DateTime? _endTime;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _tournamentControllerClient = new TournamentControllerClient(httpClient);
        }

        private async Task Create()
        {
            //TODO: add validation
            var tournamentArguments = new CreateCodeMarathonTournamentArguments
            {
                Name = _name,
                Description = _description,
                StartTime = _startTime.Value,
                EndTime = _endTime.Value,
                //TODO: implement selecting
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
            };

            await _tournamentControllerClient.CreateCodeMarathon(tournamentArguments);
        }
    }
}
