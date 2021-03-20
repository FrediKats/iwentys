using System;
using System.Threading.Tasks;
using Iwentys.Sdk;

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
            await RaidClient.CreateAsync(new RaidCreateArguments
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
