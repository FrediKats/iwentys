using System.Threading.Tasks;
using Iwentys.Features.Raids.Models;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class RaidProfilePage
    {
        private RaidProfileDto _raid;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _raid = await ClientHolder.Raid.Get(RaidId);
        }
    }
}
