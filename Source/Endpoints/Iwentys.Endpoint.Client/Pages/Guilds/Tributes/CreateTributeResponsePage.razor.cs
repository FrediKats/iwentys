using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
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

            _tribute = await ClientHolder.ApiGuildTributeAsync(TributeId);
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
            
            await ClientHolder.ApiGuildTributeCompleteAsync(tributeCompleteRequest);
            NavigationManager.NavigateTo($"/guild/{GuildId}/tribute");
        }
    }
}
