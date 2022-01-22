using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Iwentys.WebClient.Content;

public partial class AddMentorPage
{
    private class Group
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
        
    [Parameter] public int SubjectId { get; set; }
        
    [Inject] public ISnackbar Snackbar { get; set; }
        
    private int _mentorId;
    private string _groupName;
    private List<Group> _groups = new List<Group>();
    private SubjectProfileDto _subjectProfile;

    protected override async Task OnInitializedAsync()
    {
        _subjectProfile = await _subjectClient.GetSubjectByIdAsync(SubjectId);
    }

    private void ShowError(string message)
    {
        Snackbar.Add(message, Severity.Error);
    }

    private async Task AddGroup()
    {
        if (_groups.Any(g => g.Name == _groupName.Substring(0,5)))
        {
            Snackbar.Add("Group already added", Severity.Error);
            return;
        }
        try
        {
            var group = await _studyGroupClient.GetByGroupNameAsync(_groupName);
            _groups.Add(new Group()
            {
                Name = group.GroupName,
                Id = group.Id
            });
            _groupName = String.Empty;
            StateHasChanged();
        }
        catch (ApiException)
        {
            ShowError("Invalid group name");
        }
    }
        
    private void RemoveGroup(MudChip chip)
    {
        int id = (int)chip.Tag;
        _groups.Remove(_groups.First(g => g.Id == id));
        StateHasChanged();
    }

    private async Task Create()
    {
        if (!_groups.Any())
        {
            ShowError("Can't add mentor without group");
            return;
        }

        var createArgs = new SubjectMentorCreateArgs()
        {
            MentorId = _mentorId,
            GroupSubjectIds = _groups.Select(g=>g.Id).ToList(),
            SubjectId = SubjectId
        };

        try
        {
            await _mentorsManagementClient.AddMentorAsync(createArgs);
            _navigationManager.NavigateTo("/account-management/mentors/");
        }
        catch (ApiException)
        {
            ShowError("Error while adding mentor.");
        }
    }
}