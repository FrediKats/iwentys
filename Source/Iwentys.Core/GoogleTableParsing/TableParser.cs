using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;

namespace Iwentys.Core.GoogleTableParsing
{
    public class TableParser : ITableParser
    {
        private SheetsService _service;
        private ValueRange _data;
        private TableStringHelper _helper;
        public TableParser(SheetsService service, string id, string sheetName,
            int firstRow, int lastRow, string groupColumn, string nameColumn, string scoreColumn)
        {
            _service = service;

            _helper = new TableStringHelper(id, sheetName, firstRow, lastRow, groupColumn, nameColumn, scoreColumn);
        }

        private void InitDataFromTable()
        {
            if (_data == null)
            {
                var request = _service.Spreadsheets.Values.Get(_helper.Id, _helper.Range);
                _data = request.Execute();
            }
        }

        public List<StudentSubjectScore> GetStudentsList()
        {
            InitDataFromTable();

            var result = new List<StudentSubjectScore>();
            foreach (var row in _data.Values)
            {
                var group = row[_helper.GroupColumnNum];
                var name = row[_helper.NameColumnNum];
                var score = row[_helper.ScoreColumnNum];
                if (group != null && name != null && score != null)
                {
                    result.Add(new StudentSubjectScore(
                        row[_helper.GroupColumnNum].ToString(),
                        row[_helper.NameColumnNum].ToString(),
                        row[_helper.ScoreColumnNum].ToString()));
                }
                else
                {
                    //TODO: add logging
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
