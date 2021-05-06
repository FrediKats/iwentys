using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Infrastructure.DataAccess.Seeding.EntityGenerators;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Seeding
{
    public class DatabaseContextGenerator
    {
        private readonly List<IEntityGenerator> _generators = new List<IEntityGenerator>();

        public DatabaseContextGenerator()
        {
            StudyEntitiesGenerator = new StudyEntitiesGenerator().To(Register);
            StudentGenerator = new StudentGenerator(StudyEntitiesGenerator.StudyGroups).To(Register);
            GithubDataGenerator = new GithubDataGenerator(StudentGenerator.Students).To(Register);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students, GithubDataGenerator.GithubProjectEntities).To(Register);
            AchievementGenerator = new AchievementGenerator(StudentGenerator.Students, GuildGenerator.Guilds).To(Register);
            SubjectActivityGenerator = new SubjectActivityGenerator(StudyEntitiesGenerator.GroupSubjects, StudentGenerator.Students).To(Register);
            AssignmentGenerator = new AssignmentGenerator(StudentGenerator.Students).To(Register);
            SubjectAssignmentGenerator = new SubjectAssignmentGenerator(StudentGenerator.Students, StudyEntitiesGenerator.StudyGroups, StudyEntitiesGenerator.Subjects, AssignmentGenerator).To(Register);
            NewsfeedGenerator = new NewsfeedGenerator(StudentGenerator.Students, GuildGenerator.Guilds, StudyEntitiesGenerator.Subjects).To(Register);
            QuestGenerator = new QuestGenerator(StudentGenerator.Students).To(Register);
            TournamentGenerator = new TournamentGenerator(StudentGenerator.Students, GuildGenerator.Guilds, GuildGenerator.GuildMembers).To(Register);
            RaidGenerator = new RaidGenerator(StudentGenerator.Students).To(Register);
        }

        public StudyEntitiesGenerator StudyEntitiesGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
        public AchievementGenerator AchievementGenerator { get; set; }
        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public SubjectAssignmentGenerator SubjectAssignmentGenerator { get; set; }
        public AssignmentGenerator AssignmentGenerator { get; set; }
        public GithubDataGenerator GithubDataGenerator { get; set; }
        public NewsfeedGenerator NewsfeedGenerator { get; set; }
        public QuestGenerator QuestGenerator { get; set; }
        public TournamentGenerator TournamentGenerator { get; set; }
        public RaidGenerator RaidGenerator { get; set; }


        public void Seed(ModelBuilder modelBuilder)
        {
            _generators.ForEach(eg => eg.Seed(modelBuilder));
        }

        private T Register<T>(T entityGenerator) where T : IEntityGenerator
        {
            _generators.Add(entityGenerator);
            return entityGenerator;
        }
    }
}