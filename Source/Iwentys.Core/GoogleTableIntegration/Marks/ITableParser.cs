using System.Collections.Generic;

namespace Iwentys.Core.GoogleTableIntegration.Marks
{
    public interface ITableParser
    {
        List<StudentSubjectScore> GetStudentsList();
    }
}
