﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;
using MudBlazor;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentSubmitJournalPage
    {
        private IEnumerable<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;
        private IEnumerable<SubjectAssignmentSubmitDto> _tableSubjectAssignmentSubmits;
        private string _searchString = "";
        private string _stateSelectorValue = "";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await _mentorSubjectAssignmentSubmitClient.SearchSubjectAssignmentSubmitsAsync(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = SubjectId
            });
            _tableSubjectAssignmentSubmits = new List<SubjectAssignmentSubmitDto>(_subjectAssignmentSubmits);
        }

        private void NavigateToSubmitPage(object row)
        {
            //TODO: replace exception
            if (row is not SubjectAssignmentSubmitDto submit)
                throw new Exception("Something goes wrong.");

            _navigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{submit.Id}");
        }
        

        private bool IsMatchedWithSearchRequest(SubjectAssignmentSubmitDto assignment)
        {
            bool DateRangeIsOk(DateRange dateRange, DateTime? date){
                if (dateRange == null || dateRange.End == null && dateRange.Start == null)
                {
                    return true;
                }
                if (date != null && dateRange.Start.Value.Date <= date.Value.Date && dateRange.End.Value.Date >= date.Value.Date)
                {
                    return true;
                }
                return false;
            }
            
            bool searched = string.IsNullOrWhiteSpace(_searchString) ||
                            assignment.SubjectAssignmentTitle.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                            assignment.Student.SecondName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                            assignment.Student.FirstName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                            $"{assignment.SubmitTimeUtc} {assignment.RejectTimeUtc} {assignment.ApproveTimeUtc}".Contains(_searchString, StringComparison.OrdinalIgnoreCase);
            bool dateRangeIsOk = DateRangeIsOk(_approveDatePicker.DateRange, assignment.ApproveTimeUtc) &&
                                 DateRangeIsOk(_rejectDatePicker.DateRange, assignment.RejectTimeUtc) &&
                                 DateRangeIsOk(_submitDatePicker.DateRange, assignment.SubmitTimeUtc);
            bool selectStateIsOk = assignment.State.ToString().Equals(_stateSelectorValue) ||
                                   string.IsNullOrEmpty(_stateSelectorValue);
            
            
            return searched && dateRangeIsOk && selectStateIsOk;
        }
    }
}
