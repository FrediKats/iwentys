using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class QuestResponseCreatePage
{
    private QuestInfoDto _quest;

    private string _description;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
            
        _quest = await _questClient.GetByIdAsync(QuestId);
    }

    private async Task SendResponse()
    {
        await _questClient.SendResponseAsync(_quest.Id, new QuestResponseCreateArguments() {Description = _description });
    }
}