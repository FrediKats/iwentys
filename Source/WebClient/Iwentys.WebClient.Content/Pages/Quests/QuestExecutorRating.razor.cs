using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Quests
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
