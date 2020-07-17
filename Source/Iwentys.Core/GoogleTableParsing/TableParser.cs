using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;

namespace Iwentys.Core.GoogleTableParsing
{
    public class TableParser
    {
        private SheetsService _service;
        private ValueRange _data;
        private TableStringHelper _helper;
        public TableParser(SheetsService service, string id, string sheetName,
            int firstRow, int lastRow,
            string groupColumn, string nameColumn, string scoreColumn)
        {
            _service = service;

            _helper = new TableStringHelper(sheetName, firstRow, lastRow, groupColumn, nameColumn, scoreColumn);

            var request = _service.Spreadsheets.Values.Get(id, _helper.Range);

            _data = request.Execute();
        }

        private List<Student> GetStudentsList()
        {
            var result = new List<Student>();
            foreach (var row in _data.Values)
            {
                try
                {
                    result.Add(new Student()
                    {
                        Group = row[_helper.GroupColumnNum].ToString(),
                        Name = row[_helper.NameColumnNum].ToString(),
                        Score = row[_helper.ScoreColumnNum].ToString()
                    });
                }
                catch (Exception)
                {
                    // Appear when range contains empty row
                }
            }

            return result;
        }
        public string GetStudentsJson()
        {
            var result = GetStudentsList();

            return JsonConvert.SerializeObject(result);
        }
    }
}
