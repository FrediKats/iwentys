using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using MudBlazor;

namespace Iwentys.Endpoints.WebClient.Modules.AccountManagement.MentorPages
{
    public partial class MentorsJournal
    {
        private ICollection<SubjectMentorsDto> _allSubjectsMentors;
        private ICollection<SubjectMentorsDto> _subjectsMentorsToShow;

        private bool _showOnlyMySubjects;

        public bool ShowOnlyMySubjects
        {
            get => _showOnlyMySubjects;

            set
            {
                _showOnlyMySubjects = value;
                if (!_showOnlyMySubjects)
                {
                    _subjectsMentorsToShow = _allSubjectsMentors;
                }
                else
                {
                    _subjectsMentorsToShow = _allSubjectsMentors.Where(
                        sm => sm.Groups.Any(g => g.LectorMentor.Id == _currentUser.Id
                                                 || g.PracticeMentors.Any(m => m.Id == _currentUser.Id))).ToList();
                }
            }
        }

        private StudentInfoDto _currentUser;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currentUser = await StudentClient.GetSelfAsync();
            _allSubjectsMentors = await MentorsManagementClient.GetAllAsync();
            _subjectsMentorsToShow = _allSubjectsMentors;
        }

        public void RemoveMentor(MudChip chip)
        {
        }

        public void ViewModeChanged()
        {
            
        }
    }
}