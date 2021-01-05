using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestResponseCreatePage
    {
        private QuestInfoDto _quest;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _quest = await ClientHolder.Quest.Get(QuestId);
        }

        private async Task SendResponse()
        {
            await ClientHolder.Quest.SendResponse(_quest.Id);
        }
    }
}
