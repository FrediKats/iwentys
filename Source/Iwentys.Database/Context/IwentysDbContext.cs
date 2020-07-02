using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public IwentysDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<BarsPointTransactionLog> BarsPointTransactionLogs { get; set; }

        public DbSet<Guild> GuildProfiles { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Quest> Quests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetCompositeKeys(modelBuilder);

            modelBuilder.Entity<Guild>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
        }

        private void SetCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new {g.CompanyId, g.WorkerId});
        }
    }
}