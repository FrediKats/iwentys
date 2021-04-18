using System.Collections.Generic;
using System.Text.Json;
using Iwentys.Common.Tools;

namespace Iwentys.Domain.Study
{
    //FYI: very bad hack. It is not logic of StudyFeature
    public class GoogleTableData
    {
        public GoogleTableData()
        {
        }

        public GoogleTableData(string id,
            string sheetName,
            string firstRow,
            string lastRow,
            string[] nameColumns,
            string scoreColumn)
        {
            Id = id;
            SheetName = sheetName;
            FirstRow = firstRow;
            LastRow = lastRow;
            ScoreColumn = scoreColumn;
            NameColumnsList = nameColumns.SelectToList(x => x);
        }

        public string Id { get; set; }
        public string SheetName { get; set; }
        public string FirstRow { get; set; }
        public string LastRow { get; set; }
        public List<string> NameColumnsList { get; set; }
        public string ScoreColumn { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}