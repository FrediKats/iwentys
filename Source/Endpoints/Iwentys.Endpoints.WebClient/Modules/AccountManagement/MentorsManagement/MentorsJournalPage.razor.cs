using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Iwentys.Endpoints.WebClient.Modules.AccountManagement.MentorsManagement
{
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
                _subjectsMentorsToShow = _showOnlyMySubjects ? _currentUserSubjects : _allSubjectsMentors;
                }
        }

        [Inject] public ISnackbar Snackbar { get; set; }
        private StudentInfoDto _currentUser;
        private List<SubjectMentorsDto> _currentUserSubjects;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currentUser = await StudentClient.GetSelfAsync();
            _allSubjectsMentors = await MentorsManagementClient.GetAllAsync();

            _currentUserSubjects = GetCurrentUserSubjects();
        }

        public void RemoveMentor(MudChip chip)
        {
            var userToDelete = (MentorIdentifier)chip.Tag;

            try
            {
                MentorsManagementClient.RemoveMentorFromGroupAsync(userToDelete.GroupSubjectId, userToDelete.Mentor.Id);

                var group = GetGroupByMentor(userToDelete);

                group.Mentors.Remove(userToDelete.Mentor);

                _currentUserSubjects = GetCurrentUserSubjects();

                ShowOnlyMySubjects = _showOnlyMySubjects;
                
                Snackbar.Add("Mentor was removed successfully", Severity.Success);
                StateHasChanged();
            }
            catch (ApiException e)
            {
                Snackbar.Add("An error occured", Severity.Error);
            }
        }

        private GroupMentorsDto GetGroupByMentor(MentorIdentifier userToDelete)
        {
            var subject = _allSubjectsMentors.First(
                sm => sm.Groups.Any(g => g.Mentors.Contains(userToDelete.Mentor)));

            var group = subject.Groups.First(
                g => g.Mentors.Contains(userToDelete.Mentor));

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
}