using Iwentys.Domain.PeerReview;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess
{
    public interface IPeerReviewDbContext
    {
        public DbSet<ProjectReviewRequest> ProjectReviewRequests { get; set; }
        public DbSet<ProjectReviewRequestInvite> ProjectReviewRequestInvites { get; set; }
    }

    public static class PeerReviewDbContextExtensions
    {
        public static void OnPeerReviewModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectReviewRequestInvite>().HasKey(rri => new { rri.ReviewRequestId, rri.ReviewerId });
        }
    }
}