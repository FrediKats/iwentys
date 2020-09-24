using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Context
{
    public class StudentMockDataReader
    {
        public List<StudentEntity> ReadFirst(int zeroGroupId)
        {
            const string filePath = "first-course.txt";

            if (!File.Exists(filePath))
                return new List<StudentEntity>();

            return File.ReadAllLines(filePath)
                .Select(TryRead)
                .Where(s => s != null)
                .ToList();

            StudentEntity TryRead(string r)
            {
                string[] elements = r.Split("\t");

                int groupNumber = int.Parse(elements[0].Substring(3, 2), CultureInfo.InvariantCulture);
                string[] names = elements[1].Split(' ', 3).ToArray();
                int isuId = int.Parse(elements[2]);

                //TODO: add tg and vk

                return StudentEntity.CreateFromIsu(isuId, names[1], names.Length == 3 ? names[2] : null, names[0], zeroGroupId + groupNumber);
            }
        }

        public List<StudentEntity> ReadSecond(int zeroGroupId)
        {
            const string secondCourseFilePath = "second-course.txt";

            if (!File.Exists(secondCourseFilePath))
                return new List<StudentEntity>();

            return File.ReadAllLines(secondCourseFilePath)
                .Select(TryRead)
                .Where(s => s != null)
                .ToList();

            StudentEntity TryRead(string r)
            {
                string[] elements = r.Split("\t");
                if (!int.TryParse(elements[2], out int isuId))
                    return null;

                int groupNumber = int.Parse(elements[1].Substring(3, 2), CultureInfo.InvariantCulture);
                string[] names = elements[0].Split(' ', 3).ToArray();

                return StudentEntity.CreateFromIsu(isuId, names[1], names.Length == 3 ? names[2] : null, names[0], zeroGroupId + groupNumber);
            }
        }

        public List<StudyGroupEntity> ReadGroups()
        {
            const string filePath = "group-list.txt";

            return File.ReadAllLines(filePath)
                .Select(s =>
                {
                    string[] elements = s.Split("\t");

                    return new StudyGroupEntity
                    {
                        GroupName = elements[1],
                        Id = int.Parse(elements[0], CultureInfo.InvariantCulture),
                        StudyCourseId = int.Parse(elements[2], CultureInfo.InvariantCulture)
                    };
                })
                .ToList();
        }
    }
}