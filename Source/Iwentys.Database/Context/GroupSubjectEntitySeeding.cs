using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Context
{
    public static class GroupSubjectEntitySeeding
    {
        public static List<GroupSubjectEntity> CreateForOop(int subjectId, List<StudyGroupEntity> studyGroups)
        {
            return new List<GroupSubjectEntity>
            {
                CreatorForOop(subjectId, "M3201", "4", "24"),
                CreatorForOop(subjectId, "M3202", "4", "27"),
                CreatorForOop(subjectId, "M3203", "4", "25"),
                CreatorForOop(subjectId, "M3204", "4", "23"),
                CreatorForOop(subjectId, "M3205", "4", "26"),
                CreatorForOop(subjectId, "M3206", "4", "21"),
                CreatorForOop(subjectId, "M3207", "4", "21"),
                CreatorForOop(subjectId, "M3208", "4", "25"),
                CreatorForOop(subjectId, "M3209", "4", "27"),
                CreatorForOop(subjectId, "M3210", "4", "18"),
                CreatorForOop(subjectId, "M3211", "4", "22"),
                CreatorForOop(subjectId, "M3212", "4", "24"),
            };

            GroupSubjectEntity CreatorForOop(int _, string groupName, string first, string last)
            {
                return new GroupSubjectEntity
                {
                    Id = DatabaseContextSetup.Create.GroupSubjectIdentifierGenerator.Next(),
                    SubjectId = subjectId,
                    StudyGroupId = studyGroups.First(s => s.GroupName == groupName).Id,
                    LectorTeacherId = 1,
                    PracticeTeacherId = 1,
                    StudySemester = StudySemester.Y20H1,
                    SerializedGoogleTableConfig = new GoogleTableData(
                            "1H75MoSvL-165x5aM-p26eFZcY57UYx0gPtOHhvpGYGw",
                            groupName,
                            first,
                            last,
                            new[] { "A" },
                            "Y")
                        .Serialize()
                };
            }
        }
    }
}