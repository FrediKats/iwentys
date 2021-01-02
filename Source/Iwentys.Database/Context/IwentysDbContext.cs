using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding;
using Iwentys.Features.AccountManagement.Entities;
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
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.SubjectAssignments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public IwentysDbContext(DbContextOptions<IwentysDbContext> options) : base(options)
        {
        }

        #region Account management

        public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }
        public DbSet<IwentysUser> IwentysUsers { get; set; }

        #endregion

        public DbSet<Student> Students { get; set; }
        public DbSet<GithubProject> StudentProjects { get; set; }
        public DbSet<GithubUser> GithubUsersData { get; set; }
        public DbSet<BarsPointTransaction> BarsPointTransactionLogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestResponse> QuestResponses { get; set; }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }

        public DbSet<Newsfeed> Newsfeeds { get; set; }
        public DbSet<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public DbSet<GuildNewsfeed> GuildNewsfeeds { get; set; }

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
            modelBuilder.Entity<GuildMember>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new {g.CompanyId, g.WorkerId});
            modelBuilder.Entity<SubjectActivity>().HasKey(s => new {SubjectForGroupId = s.GroupSubjectId, s.StudentId});
            modelBuilder.Entity<StudentAchievement>().HasKey(a => new {a.AchievementId, a.StudentId});
            modelBuilder.Entity<GuildAchievement>().HasKey(a => new {a.AchievementId, a.GuildId});
            modelBuilder.Entity<QuestResponse>().HasKey(a => new {a.QuestId, a.StudentId});
            modelBuilder.Entity<GuildTestTaskSolution>().HasKey(a => new {a.GuildId, a.StudentId});
            modelBuilder.Entity<StudentAssignment>().HasKey(a => new {a.AssignmentId, a.StudentId});
            modelBuilder.Entity<GuildRecruitmentMember>().HasKey(g => new {g.GuildRecruitmentId, g.MemberId});
            modelBuilder.Entity<SubjectNewsfeed>().HasKey(g => new {g.SubjectId, g.NewsfeedId});
            modelBuilder.Entity<GuildNewsfeed>().HasKey(g => new {g.GuildId, g.NewsfeedId});
            modelBuilder.Entity<StudentInterestTag>().HasKey(g => new {g.StudentId, g.InterestTagId});
            modelBuilder.Entity<TournamentTeamMember>().HasKey(g => new {g.TeamId, g.MemberId});
            modelBuilder.Entity<KarmaUpVote>().HasKey(g => new {g.AuthorId, g.TargetId});
            modelBuilder.Entity<RaidVisitor>().HasKey(rv => new {rv.RaidId, rv.VisitorId});
            modelBuilder.Entity<RaidInterestTag>().HasKey(rv => new {rv.RaidId, rv.InterestTagId});
            modelBuilder.Entity<RaidPartySearchRequest>().HasKey(rv => new {rv.RaidId, rv.AuthorId});
        }

        private static void SetUniqKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
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
        public DbSet<InterestTag> InterestTags { get; set; }
        public DbSet<StudentInterestTag> UserInterestTags { get; set; }
        public DbSet<KarmaUpVote> KarmaUpVotes { get; set; }
        #endregion

        #region Guilds
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<GuildPinnedProject> GuildPinnedProjects { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipantTeam> TournamentParticipantTeams { get; set; }
        public DbSet<TournamentTeamMember> TournamentTeamMembers { get; set; }
        public DbSet<CodeMarathonTournament> CodeMarathonTournaments { get; set; }
        public DbSet<Tribute> Tributes { get; set; }
        public DbSet<GuildTestTaskSolution> GuildTestTaskSolvingInfos { get; set; }
        public DbSet<GuildRecruitment> GuildRecruitment { get; set; }
        public DbSet<GuildRecruitmentMember> GuildRecruitmentMembers { get; set; }
        #endregion

        #region Study
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<GroupSubject> GroupSubjects { get; set; }
        public DbSet<StudyCourse> StudyCourses { get; set; }

        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        #endregion

        #region Achievement
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<StudentAchievement> StudentAchievements { get; set; }
        public DbSet<GuildAchievement> GuildAchievements { get; set; }
        #endregion

        #region Raids
        public DbSet<Raid> Raids { get; set; }
        public DbSet<RaidVisitor> RaidVisitors { get; set; }
        public DbSet<RaidInterestTag> RaidInterestTags { get; set; }
        public DbSet<RaidPartySearchRequest> PartySearchRequests { get; set; }
        #endregion
    }
}