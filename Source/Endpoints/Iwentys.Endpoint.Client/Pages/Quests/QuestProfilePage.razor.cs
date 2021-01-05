using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestProfilePage
    {
        private QuestInfoDto _quest;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _quest = await ClientHolder.Quest.Get(QuestId);
        }

        private async Task AcceptQuestResponse(QuestResponseInfoDto questResponse)
        {
            await ClientHolder.Quest.Complete(_quest.Id, questResponse.Student.Id);
        }
    }
}
