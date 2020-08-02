using Iwentys.Models.Types;
using Newtonsoft.Json;

namespace Iwentys.Models.Entities.Study
{
    public class SubjectForGroup
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }

        public int TeacherId { get; set; }
        public Teacher Lecturer { get; set; }

        public string SerializedGoogleTableConfig { get; set; }

        public GoogleTableData GetGoogleTableDataConfig => JsonConvert.DeserializeObject<GoogleTableData>(SerializedGoogleTableConfig);

        public StudySemester StudySemester { get; set; }
    }
}