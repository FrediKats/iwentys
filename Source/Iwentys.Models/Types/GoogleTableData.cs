using System.Collections.Generic;
using Iwentys.Models.Tools;
using Newtonsoft.Json;

namespace Iwentys.Models.Types
{
    public class GoogleTableData
    {
        public string Id { get; set; }
        public string SheetName { get; set; }
        public string FirstRow { get; set; }
        public string LastRow { get; set; }
        public bool GroupDefined { get; set; }
        public string GroupName { get; set; }
        public string GroupColumn { get; set; }
        public List<string> NameColumnsList { get; set; }
        public string ScoreColumn { get; set; }

        public GoogleTableData()
        {
        }

        public GoogleTableData(string id, string sheetName,
            string firstRow, string lastRow,
            bool groupDefined, string groupName, string groupColumn, string[] nameColumns,
            string scoreColumn)
        {
            Id = id;
            SheetName = sheetName;
            FirstRow = firstRow;
            LastRow = lastRow;
            GroupDefined = groupDefined;
            GroupName = groupName;
            GroupColumn = groupColumn;
            ScoreColumn = scoreColumn;
            NameColumnsList = nameColumns.SelectToList(x => x);
        }

        public string Serialize() => JsonConvert.SerializeObject(this);
    }
}
