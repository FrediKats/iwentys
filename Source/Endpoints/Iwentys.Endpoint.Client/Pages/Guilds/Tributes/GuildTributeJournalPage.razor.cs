using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Tributes.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
{
    public partial class GuildTributeJournalPage
    {
        private GuildControllerClient _guildControllerClient;
        private GuildTributeControllerClient _guildTributeControllerControllerClient;

        private ExtendedGuildProfileWithMemberDataDto _guild;
        private List<TributeInfoResponse> _tributes;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _guildTributeControllerControllerClient = new GuildTributeControllerClient(httpClient);
            _guild = await _guildControllerClient.Get(GuildId);
            _tributes = await _guildTributeControllerControllerClient.GetGuildTribute(GuildId);
        }
    }
}
