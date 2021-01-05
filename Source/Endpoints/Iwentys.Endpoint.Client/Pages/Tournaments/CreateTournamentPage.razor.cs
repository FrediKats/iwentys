using System;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Endpoint.Client.Pages.Tournaments
{
    public partial class CreateTournamentPage
    {
        private string _name;
        private string _description;
        private DateTime? _startTime;
        private DateTime? _endTime;

        private async Task Create()
        {
            //FYI: WI5 add data validation
            var tournamentArguments = new CreateCodeMarathonTournamentArguments
            {
                Name = _name,
                Description = _description,
                StartTime = _startTime.Value,
                EndTime = _endTime.Value,
                //FYI: WI6 support other CodeMarathonAllowedActivityType
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
            };

            await ClientHolder.Tournament.CreateCodeMarathon(tournamentArguments);
        }
    }
}
