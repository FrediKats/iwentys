using System.Collections.Generic;

namespace Iwentys.Core.GoogleTableParsing
{
    public interface ITableParser
    {
        List<StudentSubjectScore> GetStudentsList();
    }
}
