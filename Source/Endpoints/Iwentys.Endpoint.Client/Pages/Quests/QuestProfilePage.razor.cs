using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestProfilePage
    {
        private QuestInfoDto _quest;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _quest = await ClientHolder.ApiQuestsGetAsync(QuestId);
        }

        private async Task AcceptQuestResponse(QuestResponseInfoDto questResponse)
        {
            var arguments = new QuestCompleteArguments
            {
                UserId = questResponse.Student.Id,
                //TODO: implement selecting mark
                Mark = 5
            };

            await ClientHolder.ApiQuestsCompleteAsync(_quest.Id, arguments);
        }
    }
}
