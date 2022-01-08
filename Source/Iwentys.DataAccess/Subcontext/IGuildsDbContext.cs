using Iwentys.Domain.Guilds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public interface IGuildsDbContext
{
    public DbSet<Guild> Guilds { get; set; }
    public DbSet<GuildMember> GuildMembers { get; set; }
    public DbSet<GuildLastLeave> GuildLastLeaves { get; set; }
    public DbSet<GuildPinnedProject> GuildPinnedProjects { get; set; }
    public DbSet<GuildTestTaskSolution> GuildTestTaskSolvingInfos { get; set; }
    public DbSet<GuildRecruitment> GuildRecruitment { get; set; }
    public DbSet<GuildRecruitmentMember> GuildRecruitmentMembers { get; set; }
}

public static class GuildsDbContextExtensions
{
    public static void OnGuildsModelCreating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuildMember>().HasKey(g => new { g.GuildId, g.MemberId });
        modelBuilder.Entity<GuildTestTaskSolution>().HasKey(a => new { a.GuildId, StudentId = a.AuthorId });
        modelBuilder.Entity<GuildRecruitmentMember>().HasKey(g => new { g.GuildRecruitmentId, g.MemberId });

        modelBuilder.Entity<Guild>().HasIndex(g => g.Title).IsUnique();
        modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
    }
}