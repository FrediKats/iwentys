using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private IReadOnlyList<QuestInfoDto> _selectedQuest;
        private StudentInfoDto _currentStudent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _currentStudent = await ClientHolder.Student.GetSelf();
            await SelectActive();
        }
        
        private async Task SelectActive()
        {
            _selectedQuest = await ClientHolder.Quest.GetActive();
            StateHasChanged();
        }

        private async Task SelectCreated()
        {
            _selectedQuest = await ClientHolder.Quest.GetCreatedByUser();
            StateHasChanged();
        }

        private async Task SelectArchived()
        {
            _selectedQuest = await ClientHolder.Quest.GetArchived();
            StateHasChanged();
        }

        private bool IsQuestCanBeRevoked(QuestInfoDto quest)
        {
            return quest.State == QuestState.Active && quest.Author.Id == _currentStudent?.Id;
        }

        private bool IsCanResponseToQuest(QuestInfoDto quest)
        {
            return quest.State == QuestState.Active && quest.Author.Id != _currentStudent?.Id;
        }

        private async Task RevokeQuest(QuestInfoDto quest)
        {
            //TODO: refresh elements
            await ClientHolder.Quest.Revoke(quest.Id);
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
    }
}
