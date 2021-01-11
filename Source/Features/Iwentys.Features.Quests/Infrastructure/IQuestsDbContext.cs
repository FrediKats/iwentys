using Iwentys.Features.Quests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Infrastructure
{
    public interface IQuestsDbContext
    {
        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestResponse> QuestResponses { get; set; }
    }
    public static class DbContextExtensions
    {
        public static void OnQuestsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestResponse>().HasKey(a => new { a.QuestId, a.StudentId });
        }
    }
}