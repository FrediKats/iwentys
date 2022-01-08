using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class CreateTributeResponsePage
    {
        private TributeInfoResponse _tribute;
        private string _comment;
        private int _difficultLevel;
        private int _mark;
        
        protected override async Task OnInitializedAsync()
        {
            _tribute = await _guildTributeClient.GuildTributeAsync(TributeId);
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
            
            await _guildTributeClient.CompleteAsync(tributeCompleteRequest);
            _navigationManager.NavigateTo($"/guild/{GuildId}/tribute");
        }
    }
}
