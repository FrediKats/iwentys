using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Quests
{
    public partial class QuestListComponent
    {
        private string LinkToQuestProfilePage(QuestInfoDto quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoDto quest) => $"/quest/response/{quest.Id}";
        private string LinkToAuthorProfilePage(IwentysUserInfoDto author) => $"/student/profile/{author.Id}";
    }
}
