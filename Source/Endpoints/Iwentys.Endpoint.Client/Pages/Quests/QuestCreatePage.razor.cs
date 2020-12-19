using System;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestCreatePage
    {
        private QuestControllerClient _questControllerClient;

        private string _title;
        private string _description;
        private int _price;
        private DateTime? _deadline;

        protected override async Task OnInitializedAsync()
        {
            _questControllerClient = new QuestControllerClient(await Http.TrySetHeader(LocalStorage));
        }

        private async Task SendCreateRequest()
        {
            await _questControllerClient.Create(new CreateQuestRequest(_title, _description, _price, _deadline));
        }
    }
}
