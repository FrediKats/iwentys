using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestProfilePage
    {
        private QuestInfoDto _quest;

        private QuestControllerClient _questControllerClient;

        protected override async Task OnInitializedAsync()
        {
            _questControllerClient = new QuestControllerClient(await Http.TrySetHeader(LocalStorage));
            _quest = await _questControllerClient.Get(QuestId);
        }
    }
}
