using System;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

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

            _self = await StudentClient.GetSelfAsync();

            try
            {
                _guild = await GuildClient.GetByMemberIdAsync(StudentProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                _logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            try
            {
                _group = await StudyGroupClient.GetByStudentIdAsync(StudentProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                _logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            _userKarmaStatistic = await KarmaClient.GetStatisticAsync(StudentProfile.Id);
        }

        private string LinkToGuild => $"guild/profile/{_guild.Id}";
        private string LinkToGroupProfile => $"/group/profile/{_group.GroupName}";

        private Task MakeGroupAdmin()
        {
            return StudyGroupClient.MakeGroupAdminAsync(StudentProfile.Id);
        }

        private bool IsCanSendKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && !_userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task SendKarma()
        {
             await KarmaClient.SendAsync(StudentProfile.Id);
            _userKarmaStatistic = await KarmaClient.GetStatisticAsync(StudentProfile.Id);
        }

        private bool IsCanRemoveKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && _userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task RemoveKarma()
        {
            await KarmaClient.RevokeAsync(StudentProfile.Id);
            _userKarmaStatistic = await KarmaClient.GetStatisticAsync(StudentProfile.Id);
        }
    }
}
