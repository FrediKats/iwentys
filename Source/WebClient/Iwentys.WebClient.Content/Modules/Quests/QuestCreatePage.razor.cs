using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class QuestCreatePage
{
    private string _title;
    private string _description;
    private int _price;
    private DateTime? _deadline;

    private async Task SendCreateRequest()
    {
        await _questClient.CreateAsync(new CreateQuestRequest
        {
            Title = _title,
            Deadline = _deadline,
            Description = _description,
            Price = _price
        });
    }
}