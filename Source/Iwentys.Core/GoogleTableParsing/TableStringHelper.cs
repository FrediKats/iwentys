using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Types;

namespace Iwentys.Core.GoogleTableParsing
{
    internal class TableStringHelper
    {
        public string Id { get; }
        public int GroupColumnNum { get; }
        public int NameColumnNum { get; }
        public int ScoreColumnNum { get; }
        public string Range { get; }
        public List<int> NameColumns { get; }
        public bool GroupDefined { get; }
        public string GroupName { get; }

        public TableStringHelper(GoogleTableData tableData)
        {
            NameColumns = new List<int>();
            Id = tableData.Id;
            GroupDefined = tableData.GroupDefined;
            var tmpSortList = new List<string> {tableData.ScoreColumn};
            if (GroupDefined)
                GroupName = tableData.GroupName;
            else
                tmpSortList.Add(tableData.GroupColumn);
            tmpSortList.AddRange(tableData.NameColumnsList);

            string firstColumn = tmpSortList.OrderBy(s => s).First();
            string lastColumn = tmpSortList.OrderByDescending(s => s).First();

            Range = Range = $"{tableData.SheetName}!{firstColumn}{tableData.FirstRow}:{lastColumn}{tableData.LastRow}";
            if (!GroupDefined)
                GroupColumnNum = FormatStringToInt(tableData.GroupColumn) - FormatStringToInt(firstColumn);
            ScoreColumnNum = FormatStringToInt(tableData.ScoreColumn) - FormatStringToInt(firstColumn);
            foreach (var namePart in tableData.NameColumnsList)
                NameColumns.Add(FormatStringToInt(namePart) - FormatStringToInt(firstColumn));
        }

        /// <summary>
        /// Examples of conversion:
        /// "A" -> 1
        /// "AB" -> 28
        /// "ABA" -> 729
        /// </summary>
        /// <param name="str">GoogleTable format string</param>
        /// <returns>Integer associated with the given string</returns>
        private int FormatStringToInt(string str)
        {
            int result = 0;
            for (int i = str.Length - 1; i >= 0; i--)
                result += (int)Math.Pow(26, i) * (str[i] - 'A' + 1);

            return result;
        }
    }
}
