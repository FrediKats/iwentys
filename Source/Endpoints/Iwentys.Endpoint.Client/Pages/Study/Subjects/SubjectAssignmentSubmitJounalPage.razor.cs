using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.SubjectAssignments.Enums;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectAssignmentSubmitJounalPage
    {
        private List<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmits(SubjectId);
        }

        private async Task Approve(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.SubjectAssignment.SendFeedback(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Approve
            });

            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmits(SubjectId);
        }

        private async Task Reject(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.SubjectAssignment.SendFeedback(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Reject
            });

            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmits(SubjectId);
        }
    }
}
