using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iwentys.Models.Types
{
    public class GoogleTableData
    {
        public string Id { get; }
        public string SheetName { get; }
        public string FirstRow { get; }
        public string LastRow { get; }
        public bool GroupDefined { get; }
        public string GroupName { get; }
        public string GroupColumn { get; }
        public int NameColumnNum { get; }
        public List<string> NameColumnsList { get; }
        public string ScoreColumn { get; }

        public GoogleTableData(string id, string sheetName,
            string firstRow, string lastRow,
            bool groupDefined, string groupName, string groupColumn,
            int nameColumnNum, string[] nameColumns,
            string scoreColumn)
        {
            Id = id;
            SheetName = sheetName;
            FirstRow = firstRow;
            LastRow = lastRow;
            GroupDefined = groupDefined;
            GroupName = groupName;
            GroupColumn = groupColumn;
            NameColumnNum = nameColumnNum;
            ScoreColumn = scoreColumn;
            NameColumnsList = new List<string>(NameColumnNum);
            for (int i = 0; i < NameColumnNum; i++)
                NameColumnsList.Add(nameColumns[i]);
        }

        public string Serialize() => JsonConvert.SerializeObject(this);
    }
}
