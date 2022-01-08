using Iwentys.Domain.Quests;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public interface IQuestsDbContext
{
    public DbSet<Quest> Quests { get; set; }
    public DbSet<QuestResponse> QuestResponses { get; set; }
}
public static class QuestsDbContextExtensions
{
    public static void OnQuestsModelCreating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestResponse>().HasKey(a => new { a.QuestId, a.StudentId });
    }
}