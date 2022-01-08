using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Quests
{
    public partial class QuestProfilePage
    {
        private QuestInfoDto _quest;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _quest = await QuestClient.GetByIdAsync(QuestId);
        }

        private async Task AcceptQuestResponse(QuestResponseInfoDto questResponse)
        {
            var arguments = new QuestCompleteArguments
            {
                UserId = questResponse.Student.Id,
                //TODO: implement selecting mark
                Mark = 5
            };

            await QuestClient.CompleteAsync(_quest.Id, arguments);
        }
    }
}
