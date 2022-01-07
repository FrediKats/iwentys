using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Companies;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.InterestTags;
using Iwentys.Domain.Karmas;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Raids;
using Iwentys.Domain.Raids.Models;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Infrastructure.DataAccess.Subcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iwentys.Infrastructure.DataAccess
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
        IStudySubjectAssignmentsDbContext

    {
        private readonly IDbContextSeeder _seeder;

        public IwentysDbContext(DbContextOptions<IwentysDbContext> options, IDbContextSeeder seeder) : base(options)
        {
            _seeder = seeder;
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
        public DbSet<ProjectReviewFeedback> ProjectReviewFeedbacks { get; set; }
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
        public DbSet<GroupSubjectMentor> GroupSubjectMentors { get; set; }
        public DbSet<StudyCourse> StudyCourses { get; set; }

        #endregion

        #region IStudySubjectAssignmentsDbContext
        public DbSet<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.OnAchievementModelCreating();
            modelBuilder.OnAssignmentsModelCreating();
            modelBuilder.OnCompaniesModelCreating();
            modelBuilder.OnGamificationModelCreating();
            modelBuilder.OnGuildsModelCreating();
            modelBuilder.OnGuildsTournamentsModelCreating();
            modelBuilder.OnInterestTagsModelCreating();
            modelBuilder.OnNewsfeedsModelCreating();
            modelBuilder.OnPeerReviewModelCreating();
            modelBuilder.OnQuestsModelCreating();
            modelBuilder.OnRaidsModelCreating();
            modelBuilder.OnStudyModelCreating();
            modelBuilder.OnStudySubjectAssignmentsModelCreating();

            RemoveCascadeDeleting(modelBuilder);
            Seeding(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void Seeding(ModelBuilder modelBuilder)
        {
            _seeder.Seed(modelBuilder);
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