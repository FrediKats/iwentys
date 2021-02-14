using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class RaidJournalPage
    {
        private ICollection<RaidProfileDto> _raids;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _raids = await ClientHolder.ApiRaidsProfileGetAsync();
        }

        private string LinkToRaidProfile(RaidProfileDto raid) => $"/raids/profile/{raid.Id}";

    }
}
