using System.Collections.Generic;
using System.Text;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentFullProfileDto : StudentPartialProfileDto
    {
        public StudentFullProfileDto()
        {
        }

        public StudentFullProfileDto(StudentEntity student) : base(student)
        {
            Group = student.Group?.GroupName;
            SubjectActivityInfo = student.SubjectActivities.SelectToList(sa => new SubjectActivityInfoResponse(sa));
        }

        public string Group { get; set; }

        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public List<SubjectActivityInfoResponse> SubjectActivityInfo { get; set; }

        public string FormatFullInfo()
        {
            var builder = new StringBuilder();

            builder.Append(Format());
            if (!string.IsNullOrWhiteSpace(Group))
                builder.Append(" (").Append(Group).Append(')');
            if (!string.IsNullOrWhiteSpace(GithubUsername))
                builder.Append("\nGithub: ").Append(GithubUsername);

            return builder.ToString();
        }
    }
}