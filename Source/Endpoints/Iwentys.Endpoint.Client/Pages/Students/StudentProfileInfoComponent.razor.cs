using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileInfoComponent
    {
        private GuildProfileDto _guild;
        private GroupProfileResponseDto _group;
        private StudentInfoDto _self;
        private KarmaStatistic _userKarmaStatistic;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await ClientHolder.Guild.GetForMember(StudentProfile.Id);
            _group = await ClientHolder.StudyGroup.FindStudentGroup(StudentProfile.Id);
            _self = await ClientHolder.Student.GetSelf();
            _userKarmaStatistic = await ClientHolder.Karma.GetUserKarmaStatistic(StudentProfile.Id);
        }

        private string LinkToGuild => $"guild/profile/{_guild.Id}";
        private string LinkToGroupProfile => $"/group/profile/{_group.GroupName}";

        private Task MakeGroupAdmin()
        {
            return ClientHolder.StudyGroup.MakeGroupAdmin(StudentProfile.Id);
        }

        private bool IsCanSendKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && !_userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task SendKarma()
        {
             await ClientHolder.Karma.SendUserKarma(StudentProfile.Id);
            _userKarmaStatistic = await ClientHolder.Karma.GetUserKarmaStatistic(StudentProfile.Id);
        }

        private bool IsCanRemoveKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && _userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task RemoveKarma()
        {
            await ClientHolder.Karma.RemoveUserKarma(StudentProfile.Id);
            _userKarmaStatistic = await ClientHolder.Karma.GetUserKarmaStatistic(StudentProfile.Id);
        }
    }
}
