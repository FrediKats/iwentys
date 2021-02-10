namespace Iwentys.Sdk.Extensions
{
    public static class SubjectAssignmentSubmitDtoExtensions
    {
        public AssignmentSubmitState GetState(this SubjectAssignmentSubmitDto subjectAssignmentSubmitDto)
        {
            if (subjectAssignmentSubmitDto.RejectTimeUtc is not null)
                return AssignmentSubmitState.Rejected;
            if (subjectAssignmentSubmitDto.ApproveTimeUtc is not null)
                return AssignmentSubmitState.Approved;
            return AssignmentSubmitState.Open;
        }
    }
}