using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Database.Context
{
    //TODO: Remove after release

    /// <summary>
    /// Следующие 6 методов - методы вызываемые при создании базы для того,
    /// чтобы внести в нее данные о группах, направлениях и т.д.
    /// Это сделано из расчета на то, что такая информация будет редко меняться и
    /// по этому ее не нужно получать через API.
    /// TODO: Нужно создать конфиг файл для каждого подобного набора данных и получать данные оттуда, а не заполнять прямо в коде
    /// 
    /// </summary>
    /// <returns>Список объектов, которые будут помещены в базу при загрузке</returns>
    public class DatabaseContextSetup
    {
        public List<Teacher> Teachers { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<StudyGroup> StudyGroups { get; set; }
        public List<StudyStream> StudyStreams { get; set; }
        public List<StudyProgram> StudyPrograms { get; set; }
        public List<SubjectForGroup> SubjectForGroups { get; set; }

        public List<Student> Students { get; set; }
        public List<Guild> Guilds { get; set; }
        public List<GuildMember> GuildMembers { get; set; }
        public List<GuildPinnedProject> GuildPinnedProjects { get; set; }

        public DatabaseContextSetup()
        {
            InitStudyTables();
            InitStudents();
            InitGuilds();
        }

        private void InitStudyTables()
        {
            Teachers = new List<Teacher>
            {
                new Teacher {Id = 1, Name = "Жмышенко Валерий Альбертович"},
                new Teacher {Id = 2, Name = "Сухачев Денис Владимирович"}
            };

            StudyPrograms = new List<StudyProgram> {new StudyProgram {Id = 1, Name = "ИС"}};

            StudyStreams = new List<StudyStream>
            {
                Create.IsStream(StudentGraduationYear.Y20),
                Create.IsStream(StudentGraduationYear.Y21),
                Create.IsStream(StudentGraduationYear.Y22),
                Create.IsStream(StudentGraduationYear.Y23),
                Create.IsStream(StudentGraduationYear.Y24),
            };

            Subjects = new List<Subject>
            {
                new Subject {Id = 1, Name = "Алгоритмы и структуры данных"},
                new Subject {Id = 2, Name = "Дискретная математика"},
                new Subject {Id = 3, Name = "Программирование"}
            };

            List<StudyGroup> secondCourse = Create.StreamGroup(4, 2, 12);
            StudyGroups = new List<StudyGroup>()
                .Concat(Create.StreamGroup(3, 3, 9))
                .Concat(secondCourse)
                .Concat(Create.StreamGroup(5, 1, 12))
                .ToList();

            SubjectForGroups = new List<SubjectForGroup>
            {
                new SubjectForGroup
                {
                    Id = 1,
                    SubjectId = 1,
                    StudyGroupId = secondCourse[0].Id,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y19H2
                },
                new SubjectForGroup
                {
                    Id = 2,
                    SubjectId = 2,
                    StudyGroupId = secondCourse[0].Id,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y19H2
                },
                new SubjectForGroup
                {
                    Id = 3,
                    SubjectId = 3,
                    StudyGroupId = secondCourse[0].Id,
                    TeacherId = 1,
                    StudySemester = StudySemester.Y19H2
                }
            };
        }

        private void InitStudents()
        {
            StudyGroup m3101 = StudyGroups.First(g => g.GroupName == "M3201");
            StudyGroup m3105 = StudyGroups.First(g => g.GroupName == "M3205");

            Students = new List<Student>
            {
                new Student
                {
                    Id = 228617,
                    FirstName = "Фреди",
                    MiddleName = "Кисикович",
                    SecondName = "Катс",
                    Role = UserType.Admin,
                    GroupId = null,
                    GithubUsername = "InRedikaWB",
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
                    BarsPoints = Int16.MaxValue
                },
                Student.CreateFromIsu(284446, "Максим", "Бастрыкин", m3101),
                Student.CreateFromIsu(264987, "Вадим", "Гаврилов", m3101),
                Student.CreateFromIsu(286516, "Леон", "Галстян", m3101),
                Student.CreateFromIsu(284454, "Николай", "Гридинарь", m3101),
                Student.CreateFromIsu(284457, "Матвей", "Дудко", m3101),
                Student.CreateFromIsu(264275, "Аюна", "Дымчикова", m3101),
                Student.CreateFromIsu(289140, "Сергей", "Миронец", m3101),
                Student.CreateFromIsu(284479, "Илья", "Кузнецов", m3105),
            };
        }

        private void InitGuilds()
        {
            Guilds = new List<Guild>
            {
                new Guild
                {
                    Id = 1,
                    Title = "TEF.C#",
                    Bio = "Best ITMO C# community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created
                },
                new Guild
                {
                    Id = 2,
                    Title = "TEF.Java",
                    Bio = "Best ITMO Java community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created
                },
            };

            GuildPinnedProjects = new List<GuildPinnedProject>
            {
                new GuildPinnedProject
                {
                    Id = 1,
                    GuildId = 1,
                    RepositoryName = "Fluda",
                    RepositoryOwner = "InredikaWb",
                }
            };

            GuildMembers = new List<GuildMember>
            {
                new GuildMember
                {
                    GuildId = 1,
                    MemberId = 228617,
                    MemberType = GuildMemberType.Creator
                },
                new GuildMember
                {
                    GuildId = 1,
                    MemberId = 289140,
                    MemberType = GuildMemberType.Member
                },
                new GuildMember
                {
                    GuildId = 2,
                    MemberId = 284479,
                    MemberType = GuildMemberType.Creator
                }
            };
        }

        private static class Create
        {
            private static readonly IdentifierGenerator StreamIdentifierGenerator = new IdentifierGenerator();
            private static readonly IdentifierGenerator GroupIdentifierGenerator = new IdentifierGenerator();

            public static StudyStream IsStream(StudentGraduationYear year)
            {
                return new StudyStream
                {
                    Id = StreamIdentifierGenerator.Next(),
                    Name = $"ИС {year}",
                    GraduationYear = year,
                    StudyProgramId = 1
                };
            }

            public static List<StudyGroup> StreamGroup(int streamId, int course, int groupCount)
            {
                return Enumerable
                    .Range(1, groupCount)
                    .Select(g => new StudyGroup
                    {
                        Id = GroupIdentifierGenerator.Next(),
                        StudyStreamId = streamId,
                        GroupName = $"M3{course}{g:00}",
                    })
                    .ToList();
            }
        }
    }
}
