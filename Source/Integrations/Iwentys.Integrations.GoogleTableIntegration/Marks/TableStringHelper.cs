using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Study;

namespace Iwentys.Integrations.GoogleTableIntegration.Marks
{
    internal class TableStringHelper
    {
        public TableStringHelper(GoogleTableData tableData)
        {
            //FYI: Seems like opportunity for read name from different column index.
            NameColumnNum = 0;

            NameColumns = new List<int>();
            Id = tableData.Id;
            var tmpSortList = new List<string> {tableData.ScoreColumn};
            tmpSortList.AddRange(tableData.NameColumnsList);

            string firstColumn = tmpSortList.OrderBy(s => s).First();
            string lastColumn = tmpSortList.OrderByDescending(s => s).First();

            Range = $"{tableData.SheetName}!{firstColumn}{tableData.FirstRow}:{lastColumn}{tableData.LastRow}";
            ScoreColumnNum = FormatStringToInt(tableData.ScoreColumn) - FormatStringToInt(firstColumn);
            foreach (string namePart in tableData.NameColumnsList)
                NameColumns.Add(FormatStringToInt(namePart) - FormatStringToInt(firstColumn));
        }

        public string Id { get; }
        public int NameColumnNum { get; }
        public int ScoreColumnNum { get; }
        public string Range { get; }
        public List<int> NameColumns { get; }

        /// <summary>
        ///     Examples of conversion:
        ///     "A" -> 1
        ///     "AB" -> 28
        ///     "ABA" -> 729
        /// </summary>
        /// <param name="str">GoogleTable format string</param>
        /// <returns>Integer associated with the given string</returns>
        private int FormatStringToInt(string str)
        {
            var result = 0;
            for (int i = str.Length - 1; i >= 0; i--)
                result += (int) Math.Pow(26, i) * (str[i] - 'A' + 1);

            return result;
        }
    }
}