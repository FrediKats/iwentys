using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Quests
{
    public partial class QuestResponseCreatePage
    {
        private QuestInfoDto _quest;

        private string _description;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            _quest = await QuestClient.GetByIdAsync(QuestId);
        }

        private async Task SendResponse()
        {
            await QuestClient.SendResponseAsync(_quest.Id, new QuestResponseCreateArguments() {Description = _description });
        }
    }
}
