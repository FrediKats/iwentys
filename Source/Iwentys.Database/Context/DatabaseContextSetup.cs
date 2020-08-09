using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

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

        public DatabaseContextSetup()
        {
            InitStudyTables();
        }

        private void InitStudyTables()
        {
            Teachers = new List<Teacher>
            {
                new Teacher {Id = 1, Name = "Жмышенко Валерий Альбертович"},
                new Teacher {Id = 2, Name = "Сухачев Денис Владимирович"}
            };

            StudyPrograms = new List<StudyProgram> { new StudyProgram { Id = 1, Name = "ИС" } };

            StudyStreams = new List<StudyStream>
            {
                new StudyStream
                {
                    Id = 1,
                    Name = "ИС 1 поток",
                    StudySemester = StudySemester.Y20H1
                },
                new StudyStream
                {
                    Id = 2,
                    Name = "ИС 2 поток",
                    StudySemester = StudySemester.Y20H1
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
                    Id = 1, StudyProgramId = 1, StudyStreamId = 1,
                    NamePattern = "М3201", Year = 2020
                },
                new StudyGroup
                {
                    Id = 2, StudyProgramId = 1, StudyStreamId = 1,
                    NamePattern = "М3202", Year = 2020
                },
                new StudyGroup
                {
                    Id = 3, StudyProgramId = 1, StudyStreamId = 2,
                    NamePattern = "М3203", Year = 2020
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
    }
}