using Iwentys.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.PeerReview.Infrastructure
{
    public interface IPeerReviewDbContext
    {
        public DbSet<ProjectReviewRequest> ProjectReviewRequests { get; set; }
        public DbSet<ProjectReviewRequestInvite> ProjectReviewRequestInvites { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnPeerReviewModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectReviewRequestInvite>().HasKey(rri => new { rri.ReviewRequestId, rri.ReviewerId });
        }
    }
}