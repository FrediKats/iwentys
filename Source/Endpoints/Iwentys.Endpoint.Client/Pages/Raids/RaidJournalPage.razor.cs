using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Raids.Models;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class RaidJournalPage
    {
        private List<RaidProfileDto> _raids;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _raids = await ClientHolder.Raid.Get();
        }

        private string LinkToRaidProfile(RaidProfileDto raid) => $"/raids/profile/{raid.Id}";

    }
}
