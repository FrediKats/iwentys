using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class QuestJournalPage
    {
        private ICollection<QuestInfoDto> _selectedQuest;
        private StudentInfoDto _currentStudent;

        protected override async Task OnInitializedAsync()
        {
            _currentStudent = await _studentClient.GetSelfAsync();
            await SelectActive();
        }
        
        private async Task SelectActive()
        {
            _selectedQuest = await _questClient.GetActiveAsync();
            StateHasChanged();
        }

        private async Task SelectCreated()
        {
            _selectedQuest = await _questClient.GetCreatedByUserAsync();
            StateHasChanged();
        }

        private async Task SelectArchived()
        {
            _selectedQuest = await _questClient.GetArchivedAsync();
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
            await _questClient.RevokeAsync(quest.Id);
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
        private string LinkToRating() => $"quests/rating/";
    }
}
