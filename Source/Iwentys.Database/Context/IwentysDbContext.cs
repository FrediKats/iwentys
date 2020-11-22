using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding;
using Iwentys.Features.Achievements;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;

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
        public DbSet<BarsPointTransactionLog> BarsPointTransactionLogs { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyWorkerEntity> CompanyWorkers { get; set; }

        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<QuestResponseEntity> QuestResponses { get; set; }

        public DbSet<AssignmentEntity> Assignments { get; set; }
        public DbSet<StudentAssignmentEntity> StudentAssignments { get; set; }

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
            modelBuilder.Entity<GuildMemberEntity>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorkerEntity>().HasKey(g => new {g.CompanyId, g.WorkerId});
            modelBuilder.Entity<SubjectActivityEntity>().HasKey(s => new {SubjectForGroupId = s.GroupSubjectEntityId, s.StudentId});
            modelBuilder.Entity<StudentAchievementEntity>().HasKey(a => new {a.AchievementId, a.StudentId});
            modelBuilder.Entity<GuildAchievementEntity>().HasKey(a => new {a.AchievementId, a.GuildId});
            modelBuilder.Entity<QuestResponseEntity>().HasKey(a => new {a.QuestId, a.StudentId});
            modelBuilder.Entity<GuildTestTaskSolvingInfoEntity>().HasKey(a => new {a.GuildId, a.StudentId});
            modelBuilder.Entity<StudentAssignmentEntity>().HasKey(a => new {a.AssignmentId, a.StudentId});
            modelBuilder.Entity<GuildRecruitmentMemberEntity>().HasKey(g => new {g.GuildRecruitmentId, g.MemberId});
        }

        private void SetUniqKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildEntity>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMemberEntity>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorkerEntity>().HasIndex(g => g.WorkerId).IsUnique();
        }

        private void Seeding(ModelBuilder modelBuilder)
        {
            var seedData = new DatabaseContextGenerator();

            modelBuilder.Entity<StudyProgramEntity>().HasData(seedData.StudyEntitiesGenerator.StudyPrograms);
            modelBuilder.Entity<StudyCourseEntity>().HasData(seedData.StudyEntitiesGenerator.StudyCourses);
            modelBuilder.Entity<StudyGroupEntity>().HasData(seedData.StudyEntitiesGenerator.StudyGroups);
            modelBuilder.Entity<TeacherEntity>().HasData(seedData.StudyEntitiesGenerator.Teachers);
            modelBuilder.Entity<SubjectEntity>().HasData(seedData.StudyEntitiesGenerator.Subjects);
            modelBuilder.Entity<GroupSubjectEntity>().HasData(seedData.StudyEntitiesGenerator.GroupSubjects);
            modelBuilder.Entity<SubjectActivityEntity>().HasData(seedData.SubjectActivityGenerator.SubjectActivityEntities);

            modelBuilder.Entity<StudentEntity>().HasData(seedData.StudentGenerator.Students);
            modelBuilder.Entity<GuildEntity>().HasData(seedData.GuildGenerator.Guilds);
            modelBuilder.Entity<GuildMemberEntity>().HasData(seedData.GuildGenerator.GuildMembers);

            modelBuilder.Entity<AchievementEntity>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievementEntity>().HasData(seedData.AchievementGenerator.StudentAchievementModels);
            modelBuilder.Entity<GuildAchievementEntity>().HasData(seedData.AchievementGenerator.GuildAchievementModels);

            modelBuilder.Entity<AssignmentEntity>().HasData(seedData.AssignmentGenerator.Assignments);
            modelBuilder.Entity<StudentAssignmentEntity>().HasData(seedData.AssignmentGenerator.StudentAssignments);

            modelBuilder.Entity<GithubUserEntity>().HasData(seedData.GithubActivityGenerator.GithubUserEntities);

        }

        //TODO: Hack for removing cascade. Need to rework keys
        private void RemoveCascadeDeleting(ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (IMutableForeignKey fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #region Guilds

        public DbSet<GuildEntity> Guilds { get; set; }
        public DbSet<GuildMemberEntity> GuildMembers { get; set; }
        public DbSet<GuildPinnedProjectEntity> GuildPinnedProjects { get; set; }
        public DbSet<TournamentEntity> Tournaments { get; set; }
        public DbSet<TributeEntity> Tributes { get; set; }
        public DbSet<GuildTestTaskSolvingInfoEntity> GuildTestTaskSolvingInfos { get; set; }
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