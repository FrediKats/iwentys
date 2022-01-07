using System.Threading.Tasks;

namespace Iwentys.WebClient.Pages.Guilds
{
    public partial class GuildEditPage
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await GuildClient.GetAsync(GuildId);
        }
    }
}
