using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Economy.Entities;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public IwentysDbContext(DbContextOptions<IwentysDbContext> options) : base(options)
        {
        }

        //#region Github

        //public DbSet<ActivityInfo> ActivityInfos { get; set; }
        //public DbSet<ContributionFullInfo> ContributionFullInfos { get; set; }
        //public DbSet<ContributionsInfo> ContributionsInfos { get; set; }
        //public DbSet<GithubRepository> GithubRepositories { get; set; }
        //public DbSet<YearActivityInfo> YearActivityInfos { get; set; }

        //#endregion

        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<GithubProjectEntity> StudentProjects { get; set; }
        public DbSet<GithubUserEntity> GithubUsersData { get; set; }
        public DbSet<BarsPointTransactionEntity> BarsPointTransactionLogs { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyWorkerEntity> CompanyWorkers { get; set; }

        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<QuestResponseEntity> QuestResponses { get; set; }

        public DbSet<AssignmentEntity> Assignments { get; set; }
        public DbSet<StudentAssignmentEntity> StudentAssignments { get; set; }

        public DbSet<NewsfeedEntity> Newsfeeds { get; set; }
        public DbSet<SubjectNewsfeedEntity> SubjectNewsfeeds { get; set; }
        public DbSet<GuildNewsfeedEntity> GuildNewsfeeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetCompositeKeys(modelBuilder);
            SetUniqKey(modelBuilder);
            RemoveCascadeDeleting(modelBuilder);
            Seeding(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMemberEntity>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorkerEntity>().HasKey(g => new {g.CompanyId, g.WorkerId});
            modelBuilder.Entity<SubjectActivityEntity>().HasKey(s => new {SubjectForGroupId = s.GroupSubjectEntityId, s.StudentId});
            modelBuilder.Entity<StudentAchievementEntity>().HasKey(a => new {a.AchievementId, a.StudentId});
            modelBuilder.Entity<GuildAchievementEntity>().HasKey(a => new {a.AchievementId, a.GuildId});
            modelBuilder.Entity<QuestResponseEntity>().HasKey(a => new {a.QuestId, a.StudentId});
            modelBuilder.Entity<GuildTestTaskSolutionEntity>().HasKey(a => new {a.GuildId, a.StudentId});
            modelBuilder.Entity<StudentAssignmentEntity>().HasKey(a => new {a.AssignmentId, a.StudentId});
            modelBuilder.Entity<GuildRecruitmentMemberEntity>().HasKey(g => new {g.GuildRecruitmentId, g.MemberId});
            modelBuilder.Entity<SubjectNewsfeedEntity>().HasKey(g => new {g.SubjectId, g.NewsfeedId});
            modelBuilder.Entity<GuildNewsfeedEntity>().HasKey(g => new {g.GuildId, g.NewsfeedId});
            modelBuilder.Entity<StudentInterestTagEntity>().HasKey(g => new {g.StudentId, g.InterestTagId});
        }

        private static void SetUniqKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildEntity>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMemberEntity>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorkerEntity>().HasIndex(g => g.WorkerId).IsUnique();
        }

        private static void Seeding(ModelBuilder modelBuilder)
        {
            var seedData = new DatabaseContextGenerator();
            seedData.Seed(modelBuilder);
        }

        private static void RemoveCascadeDeleting(ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (IMutableForeignKey fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #region Gamification
        public DbSet<InterestTagEntity> InterestTags { get; set; }
        public DbSet<StudentInterestTagEntity> UserInterestTags { get; set; }
        #endregion

        #region Guilds
        public DbSet<GuildEntity> Guilds { get; set; }
        public DbSet<GuildMemberEntity> GuildMembers { get; set; }
        public DbSet<GuildPinnedProjectEntity> GuildPinnedProjects { get; set; }
        public DbSet<TournamentEntity> Tournaments { get; set; }
        public DbSet<CodeMarathonTournamentEntity> CodeMarathonTournaments { get; set; }
        public DbSet<TributeEntity> Tributes { get; set; }
        public DbSet<GuildTestTaskSolutionEntity> GuildTestTaskSolvingInfos { get; set; }
        public DbSet<GuildRecruitmentEntity> GuildRecruitment { get; set; }
        public DbSet<GuildRecruitmentMemberEntity> GuildRecruitmentMembers { get; set; }
        #endregion

        #region Study

        public DbSet<StudyGroupEntity> StudyGroups { get; set; }
        public DbSet<StudyProgramEntity> StudyPrograms { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<SubjectActivityEntity> SubjectActivities { get; set; }
        public DbSet<GroupSubjectEntity> GroupSubjects { get; set; }
        public DbSet<TeacherEntity> Teachers { get; set; }
        public DbSet<StudyCourseEntity> StudyCourses { get; set; }

        #endregion

        #region Achievement

        public DbSet<AchievementEntity> Achievements { get; set; }
        public DbSet<StudentAchievementEntity> StudentAchievements { get; set; }
        public DbSet<GuildAchievementEntity> GuildAchievements { get; set; }

        #endregion
    }
}