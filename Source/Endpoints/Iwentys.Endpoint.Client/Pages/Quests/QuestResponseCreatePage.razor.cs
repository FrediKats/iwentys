using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestResponseCreatePage
    {
        private QuestInfoDto _quest;

        private string _description;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _quest = await ClientHolder.ApiQuestsGetAsync(QuestId);
        }

        private async Task SendResponse()
        {
            await ClientHolder.ApiQuestsSendResponseAsync(_quest.Id, new QuestResponseCreateArguments() {Description = _description });
        }
    }
}
