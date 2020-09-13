using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public IwentysDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Guilds

        public DbSet<GuildEntity> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<GuildPinnedProject> GuildPinnedProjects { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Tribute> Tributes { get; set; }
        public DbSet<GuildTestTaskSolvingInfoEntity> GuildTestTaskSolvingInfos { get; set; }

        #endregion

        #region Study

        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<SubjectForGroup> SubjectForGroups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudyStream> StudyStreams { get; set; }

        #endregion

        #region Achievement

        public DbSet<AchievementModel> Achievements { get; set; }
        public DbSet<StudentAchievementEntity> StudentAchievements { get; set; }
        public DbSet<GuildAchievementModel> GuildAchievements { get; set; }

        #endregion

        #region Github

        public DbSet<ActivityInfo> ActivityInfos { get; set; }
        public DbSet<ContributionFullInfo> ContributionFullInfos { get; set; }
        public DbSet<ContributionsInfo> ContributionsInfos { get; set; }
        public DbSet<GithubRepository> GithubRepositories { get; set; }
        public DbSet<YearActivityInfo> YearActivityInfos { get; set; }

        #endregion

        public DbSet<Student> Students { get; set; }
        public DbSet<GithubProjectEntity> StudentProjects { get; set; }
        public DbSet<GithubUserData> GithubUsersData { get; set; }
        public DbSet<BarsPointTransactionLog> BarsPointTransactionLogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestResponseEntity> QuestResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetCompositeKeys(modelBuilder);
            SetUniqKey(modelBuilder);
            RemoveCascadeDeleting(modelBuilder);
            Seeding(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new {g.CompanyId, g.WorkerId});
            modelBuilder.Entity<SubjectActivity>().HasKey(s => new {s.SubjectForGroupId, s.StudentId});
            modelBuilder.Entity<StudentAchievementEntity>().HasKey(a => new {a.AchievementId, a.StudentId});
            modelBuilder.Entity<GuildAchievementModel>().HasKey(a => new {a.AchievementId, a.GuildId});
            modelBuilder.Entity<QuestResponseEntity>().HasKey(a => new {a.QuestId, a.StudentId});
            modelBuilder.Entity<GuildTestTaskSolvingInfoEntity>().HasKey(a => new {a.GuildId, a.StudentId});
        }

        private void SetUniqKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildEntity>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
        }

        private void Seeding(ModelBuilder modelBuilder)
        {
            var seedData = new DatabaseContextSetup();

            modelBuilder.Entity<StudyProgram>().HasData(seedData.StudyPrograms);
            modelBuilder.Entity<StudyStream>().HasData(seedData.StudyStreams);
            modelBuilder.Entity<StudyGroup>().HasData(seedData.StudyGroups);
            modelBuilder.Entity<Teacher>().HasData(seedData.Teachers);
            modelBuilder.Entity<Subject>().HasData(seedData.Subjects);
            modelBuilder.Entity<SubjectForGroup>().HasData(seedData.SubjectForGroups);
            modelBuilder.Entity<SubjectActivity>().HasData(seedData.SubjectActivitys);

            modelBuilder.Entity<Student>().HasData(seedData.Students);
            modelBuilder.Entity<GuildEntity>().HasData(seedData.Guilds);
            modelBuilder.Entity<GuildMember>().HasData(seedData.GuildMembers);
            modelBuilder.Entity<GuildPinnedProject>().HasData(seedData.GuildPinnedProjects);

            modelBuilder.Entity<AchievementModel>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievementEntity>().HasData(seedData.StudentAchievementModels);
            modelBuilder.Entity<GuildAchievementModel>().HasData(seedData.GuildAchievementModels);
        }

        //TODO: Hack for removing cascade. Need to rework keys
        private void RemoveCascadeDeleting(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}