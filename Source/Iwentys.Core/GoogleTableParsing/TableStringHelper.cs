using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iwentys.Core.GoogleTableParsing
{
    class TableStringHelper
    {
        public int GroupColumnNum { get; }
        public int NameColumnNum { get; }
        public int ScoreColumnNum { get; }
        public string Range { get; }

        public TableStringHelper(string sheetName, int startingRow, int lastRow,
            string groupColumn, string nameColumn, string scoreColumn)
        {
            var tmpSortList = new List<string>() { groupColumn, nameColumn, scoreColumn };

            string firstColumn = tmpSortList.OrderBy(s => s).First();
            string lastColumn = tmpSortList.OrderByDescending(s => s).First();

            Range = $"{sheetName}!{firstColumn}{startingRow}:{lastColumn}{lastRow}";
            GroupColumnNum = FormatStringToInt(groupColumn) - FormatStringToInt(firstColumn);
            NameColumnNum = FormatStringToInt(nameColumn) - FormatStringToInt(firstColumn);
            ScoreColumnNum = FormatStringToInt(scoreColumn) - FormatStringToInt(firstColumn);
        }

        public int FormatStringToInt(string str)
        {
            int result = 0;
            for (int i = str.Length - 1; i >= 0; i--)
                result += (int)Math.Pow(26, i) * (str[i] - 'A' + 1);

            return result;
        }
    }
}
