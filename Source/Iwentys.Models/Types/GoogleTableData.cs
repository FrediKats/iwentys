﻿using System.Collections.Generic;
using Iwentys.Models.Tools;
using Newtonsoft.Json;

namespace Iwentys.Models.Types
{
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

        public string Serialize() => JsonConvert.SerializeObject(this);
    }
}