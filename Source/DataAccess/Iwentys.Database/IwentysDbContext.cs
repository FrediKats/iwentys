using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.AccountManagement.Infrastructure;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Achievements.Infrastructure;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Infrastructure;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Infrastructure;
using Iwentys.Features.Economy.Entities;
using Iwentys.Features.Economy.Infrastructure;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Infrastructure;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Infrastructure;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Infrastructure;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Infrastructure;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Guilds.Tributes.Infrastructure;
using Iwentys.Features.InterestTags.Entities;
using Iwentys.Features.InterestTags.Infrastructure;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Infrastructure;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Infrastructure;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Infrastructure;
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Raids.Infrastructure;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.SubjectAssignments.Entities;
using Iwentys.Features.Study.SubjectAssignments.Infrastructure;
using Iwentys.Features.Voting.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iwentys.Database
{
    public class IwentysDbContext : DbContext,
        IAccountManagementDbContext,
        IAchievementDbContext,
        IAssignmentsDbContext,
        ICompaniesDbContext,
        IEconomyDbContext,
        IGamificationDbContext,
        IGithubIntegrationDbContext,
        IGuildsDbContext,
        IGuildsTournamentsDbContext,
        ITributesDbContext,
        IInterestTagsDbContext,
        INewsfeedsDbContext,
        IPeerReviewDbContext,
        IQuestsDbContext,
        IRaidsDbContext,
        IStudyDbContext,
        IStudySubjectAssignmentsDbContext,
        IVotingDbContext

    {
        public IwentysDbContext(DbContextOptions<IwentysDbContext> options) : base(options)
        {
        }

        #region IAccountManagementDbContext
        public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }
        public DbSet<UniversitySystemUserCredential> UniversitySystemUserCredentials { get; set; }
        public DbSet<IwentysUser> IwentysUsers { get; set; }
        #endregion

        #region IAchievementDbContext
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<StudentAchievement> StudentAchievements { get; set; }
        public DbSet<GuildAchievement> GuildAchievements { get; set; }
        #endregion

        #region Assignmens
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        #endregion

        #region ICompaniesDbContext
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }
        #endregion

        #region IEconomyDbContext
        public DbSet<BarsPointTransaction> BarsPointTransactionLogs { get; set; }
        #endregion

        #region IGamificationDbContext
        public DbSet<KarmaUpVote> KarmaUpVotes { get; set; }
        public DbSet<CourseLeaderboardRow> CourseLeaderboardRows { get; set; }
        #endregion

        #region IGithubIntegrationDbContext
        public DbSet<GithubProject> StudentProjects { get; set; }
        public DbSet<GithubUser> GithubUsersData { get; set; }
        #endregion

        #region IGuildsDbContext
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<GuildLastLeave> GuildLastLeaves { get; set; }
        public DbSet<GuildPinnedProject> GuildPinnedProjects { get; set; }
        public DbSet<GuildTestTaskSolution> GuildTestTaskSolvingInfos { get; set; }
        public DbSet<GuildRecruitment> GuildRecruitment { get; set; }
        public DbSet<GuildRecruitmentMember> GuildRecruitmentMembers { get; set; }
        #endregion

        #region IGuildsTournamentsDbContext
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipantTeam> TournamentParticipantTeams { get; set; }
        public DbSet<TournamentTeamMember> TournamentTeamMembers { get; set; }
        public DbSet<CodeMarathonTournament> CodeMarathonTournaments { get; set; }
        #endregion

        #region ITributesDbContext
        public DbSet<Tribute> Tributes { get; set; }
        #endregion

        #region IInterestTagsDbContext
        public DbSet<InterestTag> InterestTags { get; set; }
        public DbSet<UserInterestTag> UserInterestTags { get; set; }
        public DbSet<RaidInterestTag> RaidInterestTags { get; set; }
        #endregion

        #region INewsfeedsDbContext
        public DbSet<Newsfeed> Newsfeeds { get; set; }
        public DbSet<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public DbSet<GuildNewsfeed> GuildNewsfeeds { get; set; }
        #endregion

        #region IPeerReviewDbContext
        public DbSet<ProjectReviewRequest> ProjectReviewRequests { get; set; }
        public DbSet<ProjectReviewRequestInvite> ProjectReviewRequestInvites { get; set; }
        #endregion

        #region IQuestsDbContext
        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestResponse> QuestResponses { get; set; }
        #endregion

        #region IRaidsDbContext
        public DbSet<Raid> Raids { get; set; }
        public DbSet<RaidVisitor> RaidVisitors { get; set; }
        public DbSet<RaidPartySearchRequest> PartySearchRequests { get; set; }
        #endregion

        #region IStudyDbContext
        public DbSet<Student> Students { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<GroupSubject> GroupSubjects { get; set; }
        public DbSet<StudyCourse> StudyCourses { get; set; }
        #endregion

        #region IStudySubjectAssignmentsDbContext
        public DbSet<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(null);

            modelBuilder.OnAccountManagementModelCreating();
            modelBuilder.OnAchievementModelCreating();
            modelBuilder.OnAssignmentsModelCreating();
            modelBuilder.OnCompaniesModelCreating();
            modelBuilder.OnEconomyModelCreating();
            modelBuilder.OnGamificationModelCreating();
            modelBuilder.OnGithubIntegrationModelCreating();
            modelBuilder.OnGuildsModelCreating();
            modelBuilder.OnGuildsTournamentsModelCreating();
            modelBuilder.OnTributesModelCreating();
            modelBuilder.OnInterestTagsModelCreating();
            modelBuilder.OnNewsfeedsModelCreating();
            modelBuilder.OnPeerReviewModelCreating();
            modelBuilder.OnQuestsModelCreating();
            modelBuilder.OnRaidsModelCreating();
            modelBuilder.OnStudyModelCreating();
            modelBuilder.OnStudySubjectAssignmentsModelCreating();
            modelBuilder.OnVotingModelCreating();

            RemoveCascadeDeleting(modelBuilder);
            Seeding(modelBuilder);

            base.OnModelCreating(modelBuilder);
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
    }
}