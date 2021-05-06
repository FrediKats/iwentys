using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Guilds.Tributes
{
    public partial class CreateTributeResponsePage
    {
        private TributeInfoResponse _tribute;
        private string _comment;
        private int _difficultLevel;
        private int _mark;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _tribute = await GuildTributeClient.GuildTributeAsync(TributeId);
        }
        
        private async Task CreateResponse()
        {
            var tributeCompleteRequest = new TributeCompleteRequest()
            {
                Comment = _comment,
                DifficultLevel = _difficultLevel,
                Mark = _mark,
                TributeId = TributeId
            };
            
            await GuildTributeClient.CompleteAsync(tributeCompleteRequest);
            NavigationManager.NavigateTo($"/guild/{GuildId}/tribute");
        }
    }
}
