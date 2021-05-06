using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private ICollection<QuestInfoDto> _selectedQuest;
        private StudentInfoDto _currentStudent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _currentStudent = await StudentClient.GetSelfAsync();
            await SelectActive();
        }
        
        private async Task SelectActive()
        {
            _selectedQuest = await QuestClient.GetActiveAsync();
            StateHasChanged();
        }

        private async Task SelectCreated()
        {
            _selectedQuest = await QuestClient.GetCreatedByUserAsync();
            StateHasChanged();
        }

        private async Task SelectArchived()
        {
            _selectedQuest = await QuestClient.GetArchivedAsync();
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
            await QuestClient.RevokeAsync(quest.Id);
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
        private string LinkToRating() => $"quests/rating/";
    }
}
