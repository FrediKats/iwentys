using System;
using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestCreatePage
    {
        private string _title;
        private string _description;
        private int _price;
        private DateTime? _deadline;

        private async Task SendCreateRequest()
        {
            await ClientHolder.Quest.Create(new CreateQuestRequest(_title, _description, _price, _deadline));
        }
    }
}
