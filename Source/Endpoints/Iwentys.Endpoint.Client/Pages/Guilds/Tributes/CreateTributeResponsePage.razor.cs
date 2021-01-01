using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tributes.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
{
    public partial class CreateTributeResponsePage
    {
        private List<TributeInfoResponse> _tributes;

        private TributeInfoResponse _tribute;
        private string _comment;
        private int _difficultLevel;
        private int _mark;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //TODO: male metho for getting by id
            _tributes = await ClientHolder.GuildTribute.GetGuildTribute(GuildId);
            _tribute = _tributes.First(t => t.Project.Id == TributeId);
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
            
            await ClientHolder.GuildTribute.CompleteTribute(tributeCompleteRequest);
            NavigationManager.NavigateTo($"/guild/{GuildId}/tribute");
        }
    }
}
