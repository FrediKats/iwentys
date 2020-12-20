using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private IReadOnlyList<QuestInfoDto> _selectedQuest;

        private QuestControllerClient _questControllerClient;

        protected override async Task OnInitializedAsync()
        {
            _questControllerClient = new QuestControllerClient(await Http.TrySetHeader(LocalStorage));
        }
        
        private async Task SelectActive()
        {
            _selectedQuest = await _questControllerClient.GetActive();
            StateHasChanged();
        }

        private async Task SelectCreated()
        {
            _selectedQuest = await _questControllerClient.GetCreatedByUser();
            StateHasChanged();
        }

        private async Task SelectArchived()
        {
            _selectedQuest = await _questControllerClient.GetArchived();
            StateHasChanged();
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
    }
}
