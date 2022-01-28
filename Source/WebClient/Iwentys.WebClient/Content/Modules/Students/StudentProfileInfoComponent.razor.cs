using System;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.WebClient.Content;

public partial class StudentProfileInfoComponent
{
    private GuildProfileDto _guild;
    private StudyGroupProfileResponseDto _group;
    private StudentInfoDto _self;
    //TODO: i'm not sure it;s ok
    private Response _userKarmaStatistic;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _self = await _studentClient.GetSelfAsync();

        try
        {
            _guild = await _guildClient.GetByMemberIdAsync(StudentProfile.Id);
        }
        catch (Exception e)
        {
            //TODO: remove this hack. Implement logic for handling 404 or null value
            _logger.Log(LogLevel.Error, e, "Failed to fetch data.");
        }

        try
        {
            _group = await _studyGroupClient.GetByStudentIdAsync(StudentProfile.Id);
        }
        catch (Exception e)
        {
            //TODO: remove this hack. Implement logic for handling 404 or null value
            _logger.Log(LogLevel.Error, e, "Failed to fetch data.");
        }

        _userKarmaStatistic = await _karmaClient.GetStatisticAsync(StudentProfile.Id);
    }

    private string LinkToGuild => $"guild/profile/{_guild.Id}";
    private string LinkToGroupProfile => $"/group/profile/{_group.GroupName}";

    private Task MakeGroupAdmin()
    {
        // TODO: fix
        // return _studyGroupClient.MakeGroupAdminAsync(StudentProfile.Id);
        return Task.CompletedTask;
    }

    private bool IsCanSendKarma()
    {
        return _self?.Id != StudentProfile.Id
               && _userKarmaStatistic is not null
               && !_userKarmaStatistic.UpVotes.Contains(_self.Id);
    }

    private async Task SendKarma()
    {
        await _karmaClient.SendAsync(StudentProfile.Id);
        _userKarmaStatistic = await _karmaClient.GetStatisticAsync(StudentProfile.Id);
    }

    private bool IsCanRemoveKarma()
    {
        return _self?.Id != StudentProfile.Id
               && _userKarmaStatistic is not null
               && _userKarmaStatistic.UpVotes.Contains(_self.Id);
    }

    private async Task RemoveKarma()
    {
        await _karmaClient.RevokeAsync(StudentProfile.Id);
        _userKarmaStatistic = await _karmaClient.GetStatisticAsync(StudentProfile.Id);
    }
}