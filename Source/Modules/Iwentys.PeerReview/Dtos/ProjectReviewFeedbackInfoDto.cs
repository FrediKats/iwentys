using System;
using System.Linq.Expressions;
using Iwentys.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;

namespace Iwentys.PeerReview;

public class ProjectReviewFeedbackInfoDto
{
    public ProjectReviewFeedbackInfoDto(ProjectReviewFeedback reviewFeedback) : this()
    {
        Id = reviewFeedback.Id;
        Description = reviewFeedback.Description;
        CreationTimeUtc = reviewFeedback.CreationTimeUtc;
        Summary = reviewFeedback.Summary;
        Author = EntityManagerApiDtoMapper.Map(reviewFeedback.Author);
    }

    public ProjectReviewFeedbackInfoDto()
    {
    }

    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreationTimeUtc { get; set; }
    public ReviewFeedbackSummary Summary { get; set; }

    public IwentysUserInfoDto Author { get; set; }

    public static Expression<Func<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>> FromEntity =>
        entity => new ProjectReviewFeedbackInfoDto
        {
            Id = entity.Id,
            Description = entity.Description,
            CreationTimeUtc = entity.CreationTimeUtc,
            Summary = entity.Summary,
            Author = EntityManagerApiDtoMapper.Map(entity.Author)
        };
}