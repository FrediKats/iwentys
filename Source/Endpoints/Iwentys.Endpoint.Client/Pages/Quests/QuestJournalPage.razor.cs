using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private ICollection<QuestInfoDto> _selectedQuest;
        private StudentInfoDto _currentStudent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _currentStudent = await ClientHolder.ApiStudentSelfAsync();
            await SelectActive();
        }
        
        private async Task SelectActive()
        {
            _selectedQuest = await ClientHolder.ApiQuestsActiveAsync();
            StateHasChanged();
        }

        private async Task SelectCreated()
        {
            _selectedQuest = await ClientHolder.ApiQuestsCreatedAsync();
            StateHasChanged();
        }

        private async Task SelectArchived()
        {
            _selectedQuest = await ClientHolder.ApiQuestsArchivedAsync();
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
            await ClientHolder.ApiQuestsRevokeAsync(quest.Id);
        }

        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
        private string LinkToRating() => $"quests/rating/";
    }
}
