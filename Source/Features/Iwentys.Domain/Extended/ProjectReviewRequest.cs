using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Enums;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Extended
{
    public class ProjectReviewRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public ProjectReviewVisibility Visibility { get; set; }
        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public long ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public virtual ICollection<ProjectReviewRequestInvite> ProjectReviewRequestInvites { get; set; }
        public virtual ICollection<ProjectReviewFeedback> ReviewFeedbacks { get; set; }

        public ProjectReviewRequest()
        {
            ProjectReviewRequestInvites = new List<ProjectReviewRequestInvite>();
            ReviewFeedbacks = new List<ProjectReviewFeedback>();
        }

        public static Expression<Func<ProjectReviewRequest, bool>> IsVisibleTo(AuthorizedUser user) =>
            request => request.Visibility == ProjectReviewVisibility.Open
            || request.AuthorId == user.Id
            || request.ProjectReviewRequestInvites.Any(rri => rri.ReviewerId == user.Id);

        public static ProjectReviewRequest CreateGuildReviewRequest(IwentysUser author, GithubRepositoryInfoDto githubProject, GuildTestTaskSolution testTaskSolution, Guild guild)
        {
            if (testTaskSolution.GetState() == GuildTestTaskState.Completed)
                throw new InnerLogicException("Task already completed");

            if (testTaskSolution.ProjectReviewRequest is not null)
                throw InnerLogicException.PeerReviewExceptions.ProjectAlreadyOnReview(githubProject.Id);

            var createArguments = new ReviewRequestCreateArguments
            {
                ProjectId = githubProject.Id,
                Description = "Guild test task review",
                Visibility = ProjectReviewVisibility.Closed
            };

            ProjectReviewRequest projectReviewRequest = Create(author, githubProject, createArguments);

            testTaskSolution.SendSubmit(author, projectReviewRequest);

            foreach (GuildMember member in guild.Members)
            {
                if (member.MemberId == author.Id)
                    continue;

                if (!member.MemberType.IsMentor())
                    continue;

                projectReviewRequest.ProjectReviewRequestInvites.Add(projectReviewRequest.InviteToReview(author, member.Member));
            }

            return projectReviewRequest;
        }

        public static ProjectReviewRequest Create(IwentysUser author, GithubRepositoryInfoDto githubProject, ReviewRequestCreateArguments createArguments)
        {
            if (githubProject.Owner != author.GithubUsername)
                throw new InnerLogicException("Project do not belong to user");

            return new ProjectReviewRequest
            {
                Description = createArguments.Description,
                State = ProjectReviewState.Requested,
                CreationTimeUtc = DateTime.UtcNow,
                LastUpdateTimeUtc = DateTime.UtcNow,
                Visibility = createArguments.Visibility,
                ProjectId = githubProject.Id,
                AuthorId = author.Id
            };
        }

        public ProjectReviewFeedback CreateFeedback(AuthorizedUser author, ReviewFeedbackCreateArguments createArguments)
        {
            if (State == ProjectReviewState.Finished)
                throw InnerLogicException.PeerReviewExceptions.ReviewAlreadyClosed(Id);

            LastUpdateTimeUtc = DateTime.UtcNow;

            return new ProjectReviewFeedback
            {
                AuthorId = author.Id,
                Description = createArguments.Description,
                Summary = createArguments.Summary,
                CreationTimeUtc = DateTime.UtcNow,
                ReviewRequestId = Id
            };
        }

        public void FinishReview(IwentysUser user)
        {
            if (user.Id != AuthorId && !user.IsAdmin) throw InnerLogicException.NotEnoughPermissionFor(user.Id);

            if (State == ProjectReviewState.Finished)
                throw InnerLogicException.PeerReviewExceptions.ReviewAlreadyClosed(Id);

            State = ProjectReviewState.Finished;
        }

        public ProjectReviewRequestInvite InviteToReview(IwentysUser requestAuthor, IwentysUser invitedUser)
        {
            if (Visibility == ProjectReviewVisibility.Open)
                throw new InnerLogicException("Request is open.");

            if (AuthorId != requestAuthor.Id)
                throw new InnerLogicException("User is not author.");

            return new ProjectReviewRequestInvite
            {
                ReviewRequestId = Id,
                ReviewerId = invitedUser.Id
            };
        }
    }
}