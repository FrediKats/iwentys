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

            _self = await ClientHolder.ApiStudentSelfAsync();

            try
            {
                _guild = await ClientHolder.ApiGuildForMemberAsync(StudentProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                Logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            try
            {
                _group = await ClientHolder.ApiStudygroupByStudentAsync(StudentProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                Logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            _userKarmaStatistic = await ClientHolder.ApiKarmaGetAsync(StudentProfile.Id);
        }

        private string LinkToGuild => $"guild/profile/{_guild.Id}";
        private string LinkToGroupProfile => $"/group/profile/{_group.GroupName}";

        private Task MakeGroupAdmin()
        {
            return ClientHolder.ApiStudygroupPromoteAdminAsync(StudentProfile.Id);
        }

        private bool IsCanSendKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && !_userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task SendKarma()
        {
             await ClientHolder.ApiKarmaPutAsync(StudentProfile.Id);
            _userKarmaStatistic = await ClientHolder.ApiKarmaGetAsync(StudentProfile.Id);
        }

        private bool IsCanRemoveKarma()
        {
            return _self?.Id != StudentProfile.Id
                   && _userKarmaStatistic is not null
                   && _userKarmaStatistic.UpVotes.Contains(_self.Id);
        }

        private async Task RemoveKarma()
        {
            await ClientHolder.ApiKarmaDeleteAsync(StudentProfile.Id);
            _userKarmaStatistic = await ClientHolder.ApiKarmaGetAsync(StudentProfile.Id);
        }
    }
}
