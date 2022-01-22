using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class QuestExecutorRating
{
    private ICollection<QuestRatingRow> _questExecutorRating;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _questExecutorRating = await _questClient.GetQuestExecutorRatingAsync();
    }

    private string LinkToProfile(IwentysUserInfoDto user) => $"student/profile/{user.Id}";

}