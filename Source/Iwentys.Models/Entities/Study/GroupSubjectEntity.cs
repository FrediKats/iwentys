using Iwentys.Models.Types;
using Newtonsoft.Json;

namespace Iwentys.Models.Entities.Study
{
    public class GroupSubjectEntity
    {
        //TODO: remove
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }

        public int StudyGroupId { get; set; }
        public StudyGroupEntity StudyGroup { get; set; }

        public int TeacherId { get; set; }
        public TeacherEntity Teacher { get; set; }

        public string SerializedGoogleTableConfig { get; set; }
        public StudySemester StudySemester { get; set; }

        public GoogleTableData GetGoogleTableDataConfig()
        {
            //TODO: remove this hack
            if (SerializedGoogleTableConfig is null)
                return null;
            return JsonConvert.DeserializeObject<GoogleTableData>(SerializedGoogleTableConfig);
        }
    }
}