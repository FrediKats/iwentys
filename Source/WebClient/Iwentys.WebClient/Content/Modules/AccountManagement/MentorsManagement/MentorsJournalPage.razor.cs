using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Iwentys.WebClient.Content;

public partial class MentorsJournalPage
{
    private class MentorIdentifier
    {
        public int GroupSubjectId { get; set; }
        public MentorDto Mentor { get; set; }
    }
        
    private ICollection<SubjectMentorsDto> _allSubjectsMentors;
    private ICollection<SubjectMentorsDto> _subjectsMentorsToShow;

    private bool _showOnlyMySubjects;

    public bool ShowOnlyMySubjects
    {
        get => _showOnlyMySubjects;

        set
        {
            _showOnlyMySubjects = value;
            _subjectsMentorsToShow = _showOnlyMySubjects 
                ? _currentUserSubjects 
                : _allSubjectsMentors;
        }
    }

    [Inject] public ISnackbar Snackbar { get; set; }
    private StudentInfoDto _currentUser;
    private List<SubjectMentorsDto> _currentUserSubjects;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await _studentClient.GetSelfAsync();
        _allSubjectsMentors = await _mentorsManagementClient.GetAllAsync();

        _currentUserSubjects = GetCurrentUserSubjects();
        _subjectsMentorsToShow = _allSubjectsMentors;
    }

    private void RemoveMentor(MudChip chip)
    {
        var mentorToDeleteIdentifier = (MentorIdentifier)chip.Tag;

        try
        {
            _mentorsManagementClient.RemoveMentorFromGroupAsync(mentorToDeleteIdentifier.GroupSubjectId, mentorToDeleteIdentifier.Mentor.Id);

            var group = GetGroupByMentor(mentorToDeleteIdentifier);

            group.Mentors.Remove(mentorToDeleteIdentifier.Mentor);

            _currentUserSubjects = GetCurrentUserSubjects();

            ShowOnlyMySubjects = _showOnlyMySubjects;
                
            Snackbar.Add("Mentor was removed successfully", Severity.Success);
            StateHasChanged();
        }
        catch (ApiException)
        {
            Snackbar.Add("An error occured", Severity.Error);
        }
    }

    private GroupMentorsDto GetGroupByMentor(MentorIdentifier mentorToDeleteIdentifier)
    {
        var subject = _allSubjectsMentors.First(
            sm=>sm.HasMentor(mentorToDeleteIdentifier.Mentor));

        var group = subject.Groups.First(
            g => g.HasMentor(mentorToDeleteIdentifier.Mentor));

        return group;
    }

    private List<SubjectMentorsDto> GetCurrentUserSubjects()
    {
        return _allSubjectsMentors.Where(
            sm => sm.Groups.Any(
                g => g.Mentors.Any(m=>m.Id == _currentUser.Id))).ToList();
    }

    public string LinkToAddMentor(int subjectId)
    {
        return $"account-management/mentors/add/{subjectId}/";
    }
}