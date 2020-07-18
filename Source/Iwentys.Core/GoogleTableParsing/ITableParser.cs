using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Sheets.v4.Data;

namespace Iwentys.Core.GoogleTableParsing
{
    public interface ITableParser
    {
        List<StudentSubjectScore> GetStudentsList();
    }
}
