using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
{
    public partial class GuildTributeJournalPage
    {
        private GuildProfileDto _guild;
        private ICollection<TributeInfoResponse> _tributes;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _guild = await GuildClient.GetAsync(GuildId);
            _tributes = await GuildTributeClient.GetByGuildIdAsync(GuildId);
        }
    }
}
