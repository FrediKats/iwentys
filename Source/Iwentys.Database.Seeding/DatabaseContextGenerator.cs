using Iwentys.Database.Seeding.EntityGenerators;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            StudyEntitiesGenerator = new StudyEntitiesGenerator();
            StudentGenerator = new StudentGenerator(StudyEntitiesGenerator.StudyGroups);
            GithubDataGenerator = new GithubDataGenerator(StudentGenerator.Students);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students, GithubDataGenerator.GithubProjectEntities);
            AchievementGenerator = new AchievementGenerator(StudentGenerator.Students, GuildGenerator.Guilds);
            SubjectActivityGenerator = new SubjectActivityGenerator(StudyEntitiesGenerator.GroupSubjects, StudentGenerator.Students);
            AssignmentGenerator = new AssignmentGenerator(StudentGenerator.Students);
            NewsfeedGenerator = new NewsfeedGenerator(StudentGenerator.Students, GuildGenerator.Guilds, StudyEntitiesGenerator.Subjects);
            QuestGenerator = new QuestGenerator(StudentGenerator.Students);
            TournamentGenerator = new TournamentGenerator(StudentGenerator.Students, GuildGenerator.Guilds, GuildGenerator.GuildMembers);
        }

        public StudyEntitiesGenerator StudyEntitiesGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
        public AchievementGenerator AchievementGenerator { get; set; }
        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public AssignmentGenerator AssignmentGenerator { get; set; }
        public GithubDataGenerator GithubDataGenerator { get; set; }
        public NewsfeedGenerator NewsfeedGenerator { get; set; }
        public QuestGenerator QuestGenerator { get; set; }
        public TournamentGenerator TournamentGenerator { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyProgram>().HasData(StudyEntitiesGenerator.StudyPrograms);
            modelBuilder.Entity<StudyCourse>().HasData(StudyEntitiesGenerator.StudyCourses);
            modelBuilder.Entity<StudyGroup>().HasData(StudyEntitiesGenerator.StudyGroups);
            modelBuilder.Entity<Teacher>().HasData(StudyEntitiesGenerator.Teachers);
            modelBuilder.Entity<Subject>().HasData(StudyEntitiesGenerator.Subjects);
            modelBuilder.Entity<GroupSubject>().HasData(StudyEntitiesGenerator.GroupSubjects);
            modelBuilder.Entity<SubjectActivity>().HasData(SubjectActivityGenerator.SubjectActivityEntities);

            modelBuilder.Entity<Student>().HasData(StudentGenerator.Students);
            modelBuilder.Entity<Guild>().HasData(GuildGenerator.Guilds);
            modelBuilder.Entity<GuildMember>().HasData(GuildGenerator.GuildMembers);
            modelBuilder.Entity<GuildPinnedProject>().HasData(GuildGenerator.PinnedProjects);
            modelBuilder.Entity<Tribute>().HasData(GuildGenerator.TributeEntities);

            modelBuilder.Entity<Achievement>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievement>().HasData(AchievementGenerator.StudentAchievementModels);
            modelBuilder.Entity<GuildAchievement>().HasData(AchievementGenerator.GuildAchievementModels);

            modelBuilder.Entity<Assignment>().HasData(AssignmentGenerator.Assignments);
            modelBuilder.Entity<StudentAssignment>().HasData(AssignmentGenerator.StudentAssignments);

            modelBuilder.Entity<GithubUser>().HasData(GithubDataGenerator.GithubUserEntities);
            modelBuilder.Entity<GithubProject>().HasData(GithubDataGenerator.GithubProjectEntities);

            modelBuilder.Entity<Newsfeed>().HasData(NewsfeedGenerator.Newsfeeds);
            modelBuilder.Entity<SubjectNewsfeed>().HasData(NewsfeedGenerator.SubjectNewsfeeds);
            modelBuilder.Entity<GuildNewsfeed>().HasData(NewsfeedGenerator.GuildNewsfeeds);

            modelBuilder.Entity<Quest>().HasData(QuestGenerator.Quest);
            modelBuilder.Entity<QuestResponse>().HasData(QuestGenerator.QuestResponse);

            modelBuilder.Entity<Tournament>().HasData(TournamentGenerator.Tournaments);
            modelBuilder.Entity<CodeMarathonTournament>().HasData(TournamentGenerator.CodeMarathonTournaments);
            modelBuilder.Entity<TournamentParticipantTeam>().HasData(TournamentGenerator.TournamentParticipantTeams);
            modelBuilder.Entity<TournamentTeamMember>().HasData(TournamentGenerator.TournamentTeamMember);
        }
    }
}