using System;
using System.Collections.Generic;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
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
                new StudyStream
                {
                    Id = 1,
                    Name = "ИС y20",
                    GraduationYear = StudentGraduationYear.Y20,
                    StudyProgramId = 1
                },
                new StudyStream
                {
                    Id = 2,
                    Name = "ИС y21",
                    GraduationYear = StudentGraduationYear.Y21,
                    StudyProgramId = 1
                }
            };

            Subjects = new List<Subject>
            {
                new Subject {Id = 1, Name = "Programming"}, new Subject {Id = 2, Name = "Physical Culture"}
            };

            StudyGroups = new List<StudyGroup>
            {
                new StudyGroup
                {
                    Id = 1,
                    StudyStreamId = 1,
                    GroupName = "М3201",
                },
                new StudyGroup
                {
                    Id = 2,
                    StudyStreamId = 1,
                    GroupName = "М3202",
                },
                new StudyGroup
                {
                    Id = 3,
                    StudyStreamId = 2,
                    GroupName = "М3203",
                }
            };

            SubjectForGroups = new List<SubjectForGroup>
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
        }

        private void InitStudents()
        {
            Students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Fredi",
                    MiddleName = "String",
                    SecondName = "Kats",
                    Role = UserType.Common,
                    GroupId = 1,
                    GithubUsername = "InRedikaWB",
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
                    BarsPoints = Int16.MaxValue
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Fredi2",
                    MiddleName = "String2",
                    SecondName = "Kats2",
                    Role = UserType.Common,
                    GroupId = 1,
                    GithubUsername = "InRedikaWB",
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
                    BarsPoints = Int16.MaxValue
                }
            };
        }

        private void InitGuilds()
        {
            Guilds = new List<Guild>
            {
                new Guild
                {
                    Id = 1,
                    Title = "TEF",
                    Bio = "Best ITMO C# community!",
                    LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                    HiringPolicy = GuildHiringPolicy.Open,
                    GuildType = GuildType.Created,
                }
            };

            GuildPinnedProjects = new List<GuildPinnedProject>
            {
                new GuildPinnedProject
                {
                    GuildId = 1,
                    RepositoryName = "RepoName",
                    RepositoryOwner = "InredikaWb",
                    Id = 2
                }
            };

            GuildMembers = new List<GuildMember>
            {
                new GuildMember
                {
                    GuildId = 1,
                    MemberId = 2,
                    MemberType = GuildMemberType.Creator
                }
            };
        }
    }

}
