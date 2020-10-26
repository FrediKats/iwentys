using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;

namespace Iwentys.Integrations.GoogleTableIntegration.TeacherInfoParse
{
    public class SubjectTeacherParser : ITableRequest<List<SubjectTeacherInfo>>
    {
        public SubjectTeacherParser(string id, string range)
        {
            Id = id;
            Range = range;
        }

        public string Id { get; }
        public string Range { get; }

        public List<SubjectTeacherInfo> Parse(ValueRange values)
        {
            var result = new List<SubjectTeacherInfo>();
            IList<object> header = values.Values.First();

            foreach (IList<object> row in values.Values.Skip(1))
            {
                var subjectName = row[0].ToString();
                var type = row[1].ToString();

                for (var i = 2; i < row.Count; i++)
                {
                    var teacher = row[i].ToString();
                    var group = header[i].ToString();
                    if (!string.IsNullOrEmpty(teacher))
                        result.Add(new SubjectTeacherInfo
                        {
                            GroupName = @group,
                            Subject = subjectName,
                            TeacherName = teacher,
                            Type = type
                        });
                }
            }

            return result;
        }
    }
}