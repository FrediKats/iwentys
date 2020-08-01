using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public IwentysDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Guilds

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<GuildPinnedProject> GuildPinnedProjects { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Tribute> Tributes { get; set; }

        #endregion

        #region Study

        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<SubjectForGroup> SubjectForGroups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        #endregion

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentProject> StudentProjects { get; set; }
        public DbSet<BarsPointTransactionLog> BarsPointTransactionLogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }

        public DbSet<Quest> Quests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetCompositeKeys(modelBuilder);

            modelBuilder.Entity<Guild>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();

            modelBuilder.Entity<StudyProgram>().HasData(getStudyProgramsList());
            modelBuilder.Entity<StudyGroup>().HasData(getStudyGroupsList());
            modelBuilder.Entity<Teacher>().HasData(getTeachersList());
            modelBuilder.Entity<Subject>().HasData(getSubjectsList());
            modelBuilder.Entity<SubjectForGroup>().HasData(getSubjectForGroupsList());

            //TODO: fix
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        private void SetCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>().HasKey(g => new {g.GuildId, g.MemberId});
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new {g.CompanyId, g.WorkerId});
            modelBuilder.Entity<SubjectActivity>().HasKey(s => new {s.SubjectForGroupId, s.StudentId});
        }

        /// <summary>
        /// Следующие 5 методов - методы вызываемые при создании базы для того,
        /// чтобы внести в нее данные о группах, направлениях и т.д.
        /// Это сделано из расчета на то, что такая информация будет редко меняться и
        /// по этому ее не нужно получать через API.
        /// TODO: Нужно создать конфиг файл для каждого подобного набора данных и получать данные оттуда, а не заполнять прямо в коде
        /// 
        /// </summary>
        /// <returns>Список объектов, которые будут помещены в базу при загрузке</returns>
        private List<StudyProgram> getStudyProgramsList()
        {
            var result = new List<StudyProgram> {new StudyProgram {Id = 1, Name = "ИС"}};

            return result;
        }
        private List<StudyGroup> getStudyGroupsList()
        {
            var result = new List<StudyGroup>
            {
                new StudyGroup
                {
                    Id = 1, StudyProgramId = 1,
                    NamePattern = "М3201", Year = 2020
                },
                new StudyGroup
                {
                    Id = 2, StudyProgramId = 1,
                    NamePattern = "М3202", Year = 2020
                },
                new StudyGroup
                {
                    Id = 3, StudyProgramId = 1,
                    NamePattern = "М3203", Year = 2020
                }
            };

            return result;
        }

        private List<Subject> getSubjectsList()
        {
            var result = new List<Subject>
            {
                new Subject {Id = 1, Name = "Programming"}, new Subject {Id = 2, Name = "Physical Culture"}
            };

            return result;
        }

        private List<Teacher> getTeachersList()
        {
            var result = new List<Teacher>
            {
                new Teacher {Id = 1, Name = "Жмышенко Валерий Альбертович"},
                new Teacher {Id = 2, Name = "Сухачев Денис Владимирович"}
            };

            return result;
        }

        private List<SubjectForGroup> getSubjectForGroupsList()
        {
            var result = new List<SubjectForGroup>
            {
                new SubjectForGroup
                {
                    Id = 1,
                    SubjectId = 1,
                    StudyGroupId = 1,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y20H1
                },
                new SubjectForGroup
                {
                    Id = 2,
                    SubjectId = 2,
                    StudyGroupId = 1,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y20H1
                },
                new SubjectForGroup
                {
                    Id = 3,
                    SubjectId = 1,
                    StudyGroupId = 1,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y20H1
                }
            };

            return result;
        }
    }
}