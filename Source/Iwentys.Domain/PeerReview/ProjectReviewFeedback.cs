using System;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.PeerReview;

public class ProjectReviewFeedback
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreationTimeUtc { get; set; }
    public ReviewFeedbackSummary Summary { get; set; }

    public int ReviewRequestId { get; set; }
    public virtual ProjectReviewRequest ReviewRequest { get; set; }

    public int AuthorId { get; set; }
    public virtual IwentysUser Author { get; set; }
}