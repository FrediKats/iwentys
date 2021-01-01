using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
{
    public partial class GuildTributeJournalPage
    {
        private ExtendedGuildProfileWithMemberDataDto _guild;
        private List<TributeInfoResponse> _tributes;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _guild = await ClientHolder.Guild.Get(GuildId);
            _tributes = await ClientHolder.GuildTribute.GetGuildTribute(GuildId);
        }
    }
}
