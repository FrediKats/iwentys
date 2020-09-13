using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Iwentys.Models.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.String;

namespace Iwentys.Core.GoogleTableParsing
{
    public class TableParser : ITableParser
    {
        private readonly ILogger _logger;
        private readonly SheetsService _service;
        private readonly TableStringHelper _helper;
        private ValueRange _data;

        public TableParser(ILogger logger, SheetsService service, GoogleTableData tableData)
        {
            _logger = logger;
            _service = service;

            _helper = new TableStringHelper(tableData);
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
                var name = row[_helper.NameColumnNum];
                var score = row[_helper.ScoreColumnNum];
                if (name != null && score != null)
                {
                    string fullName = Join(" ", _helper.NameColumns.Select(c => row[c]));

                    result.Add(new StudentSubjectScore(fullName,
                        score.ToString()));
                }
                else
                {
                    _logger.LogWarning($"Missed data while parsing google table: tableId:{_helper.Id}");
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
