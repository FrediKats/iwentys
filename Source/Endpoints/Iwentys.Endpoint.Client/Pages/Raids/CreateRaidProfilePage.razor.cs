using System;
using System.Threading.Tasks;
using Iwentys.Features.Raids.Models;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class CreateRaidProfilePage
    {
        private string _name;
        private string _description;
        private DateTime? _startTime;
        private DateTime? _endTime;

        private async Task Create()
        {
            await ClientHolder.Raid.Create(new RaidCreateArguments
            {
                Title = _name,
                Description = _description,
                StartTimeUtc = _startTime.Value,
                EndTimeUtc = _endTime.Value
            });

            NavigationManager.NavigateTo("/raids");
        }
    }
}
