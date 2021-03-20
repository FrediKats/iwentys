using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestExecutorRating
    {
        private ICollection<QuestRatingRow> _questExecutorRating;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _questExecutorRating = await QuestClient.GetQuestExecutorRatingAsync();
        }

        private string LinkToProfile(IwentysUserInfoDto user) => $"student/profile/{user.Id}";

    }
}
