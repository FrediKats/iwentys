using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class SubjectAssignmentSubmitPage
{
    public class Arguments
    {
        public string Comment { get; set; }
        public FeedbackType FeedbackType { get; set; }
        [Range(-100,100,ErrorMessage = "Assignment can be evaluated  only between {1} and {2}")]
        public int? Points { get; set; }
    }

    private SubjectAssignmentSubmitDto _submit;
    private Arguments _arguments = new Arguments();
    private bool _hasReview;
    private StudentInfoDto _reviewer;
    private ConfirmationModal _confirmationModal;

    protected override async Task OnInitializedAsync()
    {
        _submit = await _mentorSubjectAssignmentSubmitClient.GetByIdAsync(SubmitId);
        _hasReview = _submit is not null && _submit.State is SubmitState.Approved or SubmitState.Rejected;
        if (_hasReview)
            _reviewer = await _studentClient.GetByIdAsync(_submit.ReviewerId);
    }

    private async Task CheckBeforeCreate()
    {
        if (_hasReview)
        {
            _confirmationModal.Show();
        }
        else
        {
            await Create();
        }
    }

    private async Task Confirm()
    {
        _confirmationModal?.Hide();
        await Create();
    }
        
    private async Task Create()
    {
        await _mentorSubjectAssignmentSubmitClient.SendSubmitFeedbackAsync(CreateArg(_arguments));
        _navigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{SubmitId}", true);
    }

    private SubjectAssignmentSubmitFeedbackArguments CreateArg(Arguments arguments)
    {
        return new SubjectAssignmentSubmitFeedbackArguments
        {
            SubjectAssignmentSubmitId = SubmitId,
            Comment = arguments.Comment,
            FeedbackType = arguments.FeedbackType,
            Points = arguments.Points
        };
    }
}