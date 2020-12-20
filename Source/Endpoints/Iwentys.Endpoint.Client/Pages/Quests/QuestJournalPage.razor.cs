using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private IReadOnlyList<QuestInfoDto> _selectedQuest;
        private StudentInfoDto _currentStudent;

        private StudentControllerClient _studentControllerClient;
        private QuestControllerClient _questControllerClient;

        protected override async Task OnInitializedAsync()
        {
            var httpClient = await Http.TrySetHeader(LocalStorage);
            _studentControllerClient = new StudentControllerClient(httpClient);
            _questControllerClient = new QuestControllerClient(httpClient);

            _currentStudent = await _studentControllerClient.GetSelf();
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

        private bool IsQuestCanBeRevoked(QuestInfoDto quest)
        {
            return quest.State == QuestState.Active && quest.Author.Id == _currentStudent?.Id;
        }
        
        private async Task RevokeQuest(QuestInfoDto quest)
        {
            //TODO: refresh elements
            await _questControllerClient.Revoke(quest.Id, _currentStudent.Id);
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
    }
}
