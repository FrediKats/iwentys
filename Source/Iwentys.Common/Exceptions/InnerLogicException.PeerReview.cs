namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class PeerReviewExceptions
        {
            public static InnerLogicException ProjectAlreadyOnReview(long projectId)
            {
                return new InnerLogicException($"Project already on review. Project: {projectId}");
            }

            public static InnerLogicException ReviewAlreadyClosed(int reviewRequestId)
            {
                return new InnerLogicException($"Review request already finished. Review request: {reviewRequestId}");
            }
        }
    }
}
