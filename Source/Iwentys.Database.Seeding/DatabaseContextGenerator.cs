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
            TournamentGenerator = new TournamentGenerator(StudentGenerator.Students);

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
            modelBuilder.Entity<StudyProgramEntity>().HasData(StudyEntitiesGenerator.StudyPrograms);
            modelBuilder.Entity<StudyCourseEntity>().HasData(StudyEntitiesGenerator.StudyCourses);
            modelBuilder.Entity<StudyGroupEntity>().HasData(StudyEntitiesGenerator.StudyGroups);
            modelBuilder.Entity<TeacherEntity>().HasData(StudyEntitiesGenerator.Teachers);
            modelBuilder.Entity<SubjectEntity>().HasData(StudyEntitiesGenerator.Subjects);
            modelBuilder.Entity<GroupSubjectEntity>().HasData(StudyEntitiesGenerator.GroupSubjects);
            modelBuilder.Entity<SubjectActivityEntity>().HasData(SubjectActivityGenerator.SubjectActivityEntities);

            modelBuilder.Entity<StudentEntity>().HasData(StudentGenerator.Students);
            modelBuilder.Entity<GuildEntity>().HasData(GuildGenerator.Guilds);
            modelBuilder.Entity<GuildMemberEntity>().HasData(GuildGenerator.GuildMembers);
            modelBuilder.Entity<GuildPinnedProjectEntity>().HasData(GuildGenerator.PinnedProjects);
            modelBuilder.Entity<TributeEntity>().HasData(GuildGenerator.TributeEntities);

            modelBuilder.Entity<AchievementEntity>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievementEntity>().HasData(AchievementGenerator.StudentAchievementModels);
            modelBuilder.Entity<GuildAchievementEntity>().HasData(AchievementGenerator.GuildAchievementModels);

            modelBuilder.Entity<AssignmentEntity>().HasData(AssignmentGenerator.Assignments);
            modelBuilder.Entity<StudentAssignmentEntity>().HasData(AssignmentGenerator.StudentAssignments);

            modelBuilder.Entity<GithubUserEntity>().HasData(GithubDataGenerator.GithubUserEntities);
            modelBuilder.Entity<GithubProjectEntity>().HasData(GithubDataGenerator.GithubProjectEntities);

            modelBuilder.Entity<NewsfeedEntity>().HasData(NewsfeedGenerator.Newsfeeds);
            modelBuilder.Entity<SubjectNewsfeedEntity>().HasData(NewsfeedGenerator.SubjectNewsfeeds);
            modelBuilder.Entity<GuildNewsfeedEntity>().HasData(NewsfeedGenerator.GuildNewsfeeds);

            modelBuilder.Entity<QuestEntity>().HasData(QuestGenerator.Quest);
            modelBuilder.Entity<QuestResponseEntity>().HasData(QuestGenerator.QuestResponse);

            modelBuilder.Entity<TournamentEntity>().HasData(TournamentGenerator.Tournaments);
            modelBuilder.Entity<CodeMarathonTournamentEntity>().HasData(TournamentGenerator.CodeMarathonTournaments);
            modelBuilder.Entity<TournamentParticipantTeamEntity>().HasData(TournamentGenerator.TournamentParticipantTeams);
            modelBuilder.Entity<TournamentTeamMemberEntity>().HasData(TournamentGenerator.TournamentTeamMember);
        }
    }
}