using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;
using Iwentys.Models.Types;

namespace Iwentys.Database.Context
{
    //TODO: Remove after release

    /// <summary>
    ///     Следующие 6 методов - методы вызываемые при создании базы для того,
    ///     чтобы внести в нее данные о группах, направлениях и т.д.
    ///     Это сделано из расчета на то, что такая информация будет редко меняться и
    ///     по этому ее не нужно получать через API.
    ///     TODO: Нужно создать конфиг файл для каждого подобного набора данных и получать данные оттуда, а не заполнять прямо
    ///     в коде
    /// </summary>
    /// <returns>Список объектов, которые будут помещены в базу при загрузке</returns>
    public class DatabaseContextSetup
    {
        public DatabaseContextSetup()
        {
            InitStudyTables();
            InitStudents();
            InitGuilds();
            InitAchievements();
        }

        public List<TeacherEntity> Teachers { get; set; }
        public List<SubjectEntity> Subjects { get; set; }
        public List<StudyGroupEntity> StudyGroups { get; set; }
        public List<StudyCourseEntity> StudyCourses { get; set; }
        public List<StudyProgramEntity> StudyPrograms { get; set; }
        public List<GroupSubjectEntity> GroupSubjects { get; set; }

        public List<StudentEntity> Students { get; set; }
        public List<GuildEntity> Guilds { get; set; }
        public List<GuildMemberEntity> GuildMembers { get; set; }
        public List<GuildPinnedProjectEntity> GuildPinnedProjects { get; set; }

        public List<GuildAchievementEntity> GuildAchievementModels { get; set; }
        public List<StudentAchievementEntity> StudentAchievementModels { get; set; }

        private void InitStudyTables()
        {
            Teachers = new List<TeacherEntity>
            {
                new TeacherEntity {Id = 1, Name = "Жмышенко Валерий Альбертович"},
                new TeacherEntity {Id = 2, Name = "Сухачев Денис Владимирович"}
            };

            StudyPrograms = new List<StudyProgramEntity> {new StudyProgramEntity {Id = 1, Name = "ИС"}};

            StudyCourses = new List<StudyCourseEntity>
            {
                Create.IsCourse(StudentGraduationYear.Y20),
                Create.IsCourse(StudentGraduationYear.Y21),
                Create.IsCourse(StudentGraduationYear.Y22),
                Create.IsCourse(StudentGraduationYear.Y23),
                Create.IsCourse(StudentGraduationYear.Y24)
            };

            Subjects = new List<SubjectEntity>
            {
                new SubjectEntity {Id = 1, Name = "Алгоритмы и структуры данных"},
                new SubjectEntity {Id = 2, Name = "Дискретная математика"},
                new SubjectEntity {Id = 3, Name = "Программирование"}
            };

            var reader = new StudentMockDataReader();
            StudyGroups = reader.ReadGroups();
            StudyGroupEntity m3201 = StudyGroups.First(g => g.GroupName == "M3201");
            StudyGroupEntity m3202 = StudyGroups.First(g => g.GroupName == "M3202");
            StudyGroupEntity m3203 = StudyGroups.First(g => g.GroupName == "M3203");

            GroupSubjects = new List<GroupSubjectEntity>
            {
                new GroupSubjectEntity
                {
                    Id = Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = 1,
                    StudyGroupId = m3201.Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y19H2
                },
                new GroupSubjectEntity
                {
                    Id = Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = 2,
                    StudyGroupId = m3201.Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y19H2
                },

                new GroupSubjectEntity
                {
                    Id = Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = 3,
                    StudyGroupId = m3201.Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y19H2,
                    SerializedGoogleTableConfig = new GoogleTableData(
                        "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                        "M3101",
                        "4",
                        "24",
                        new[] {"B"},
                        "V").Serialize()
                },

                new GroupSubjectEntity
                {
                    Id = Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = 3,
                    StudyGroupId = m3202.Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y19H2,
                    SerializedGoogleTableConfig = new GoogleTableData(
                            "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                            "M3102",
                            "4",
                            "25",
                            new[] {"B"},
                            "V")
                        .Serialize()
                },

                new GroupSubjectEntity
                {
                    Id = Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = 3,
                    StudyGroupId = m3203.Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y19H2,
                    SerializedGoogleTableConfig = new GoogleTableData(
                            "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                            "M3103",
                            "4",
                            "25",
                            new[] {"B"},
                            "V")
                        .Serialize()
                }
            };
        }

        private void InitStudents()
        {
            StudyGroupEntity m3101 = StudyGroups.First(g => g.GroupName == "M3101");
            StudyGroupEntity m3201 = StudyGroups.First(g => g.GroupName == "M3201");
            StudyGroupEntity m3305 = StudyGroups.First(g => g.GroupName == "M3305");
            StudyGroupEntity m3505 = StudyGroups.First(g => g.GroupName == "M3505");

            var reader = new StudentMockDataReader();
            List<StudentEntity> students = reader.ReadFirst(m3101.Id - 1);
            students.AddRange(reader.ReadSecond(m3201.Id - 1));
            if (students.Count == 0)
                students.AddRange(ReadStudentsFromDefault());

            Students = students
                .Append(new StudentEntity
                {
                    Id = 228617,
                    FirstName = "Фреди",
                    MiddleName = "Кисикович",
                    SecondName = "Катс",
                    Role = UserType.Admin,
                    GroupId = m3505.Id,
                    GithubUsername = "InRedikaWB",
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
                    BarsPoints = short.MaxValue
                })
                .Append(StudentEntity.CreateFromIsu(264312, "Илья", "Шамов", m3305))
                .Append(StudentEntity.CreateFromIsu(264282, "Илья", "Ильменский", m3305))
                .ToList();

            Students.Single(s => s.Id == 289140).GithubUsername = "s4xack";
        }

        private void InitGuilds()
        {
            Guilds = new List<GuildEntity>
            {
                new GuildEntity
                {
                    Id = 1,
                    Title = "TEF-Dev",
                    Bio = "Best ITMO C# community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created
                },
                new GuildEntity
                {
                    Id = 2,
                    Title = "javaica",
                    Bio = "Best ITMO Java community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created
                },
                new GuildEntity
                {
                    Id = 3,
                    Title = "CodingPenguinParty",
                    Bio = "Best ITMO ML community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created
                }
            };

            GuildPinnedProjects = new List<GuildPinnedProjectEntity>
            {
                new GuildPinnedProjectEntity
                {
                    Id = 1,
                    GuildId = 1,
                    RepositoryName = "Fluda",
                    RepositoryOwner = "InredikaWb"
                }
            };
            GuildMembers = new List<GuildMemberEntity>
            {
                new GuildMemberEntity(1, 228617, GuildMemberType.Creator),
                new GuildMemberEntity(1, 289140, GuildMemberType.Member),

                new GuildMemberEntity(2, 284479, GuildMemberType.Creator),

                new GuildMemberEntity(3, 264312, GuildMemberType.Creator),
                new GuildMemberEntity(3, 264282, GuildMemberType.Member)
            };
        }

        private void InitAchievements()
        {
            StudentAchievementModels = new List<StudentAchievementEntity>
            {
                new StudentAchievementEntity
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    StudentId = 289140,
                    GettingTime = DateTime.UtcNow
                }
            };

            GuildAchievementModels = new List<GuildAchievementEntity>
            {
                new GuildAchievementEntity
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    GuildId = 1,
                    GettingTime = DateTime.UtcNow
                }
            };
        }

        private List<StudentEntity> ReadStudentsFromDefault()
        {
            StudyGroupEntity m3201 = StudyGroups.First(g => g.GroupName == "M3201");
            StudyGroupEntity m3202 = StudyGroups.First(g => g.GroupName == "M3202");
            StudyGroupEntity m3205 = StudyGroups.First(g => g.GroupName == "M3205");

            return new List<StudentEntity>
            {
                StudentEntity.CreateFromIsu(284446, "Максим", "Бастрыкин", m3201),
                StudentEntity.CreateFromIsu(264987, "Вадим", "Гаврилов", m3201),
                StudentEntity.CreateFromIsu(286516, "Леон", "Галстян", m3201),
                StudentEntity.CreateFromIsu(284454, "Николай", "Гридинарь", m3201),
                StudentEntity.CreateFromIsu(284457, "Матвей", "Дудко", m3201),
                StudentEntity.CreateFromIsu(264275, "Аюна", "Дымчикова", m3201),
                StudentEntity.CreateFromIsu(289140, "Сергей", "Миронец", m3201),

                StudentEntity.CreateFromIsu(284441, "Ульяна", "Абрамова", m3202),
                StudentEntity.CreateFromIsu(283184, "Денис", "Андреев", m3202),
                StudentEntity.CreateFromIsu(284443, "Сергей", "Артамонов", m3202),

                StudentEntity.CreateFromIsu(284479, "Илья", "Кузнецов", m3205)
            };
        }

        private static class Create
        {
            private static readonly IdentifierGenerator CourseIdentifierGenerator = new IdentifierGenerator();
            public static readonly IdentifierGenerator GroupSubjectIdentifierGenerator = new IdentifierGenerator();

            public static StudyCourseEntity IsCourse(StudentGraduationYear year)
            {
                return new StudyCourseEntity
                {
                    Id = CourseIdentifierGenerator.Next(),
                    GraduationYear = year,
                    StudyProgramId = 1
                };
            }
        }
    }
}