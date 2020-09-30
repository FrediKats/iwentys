using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;
using Iwentys.Models.Types;
using Microsoft.Extensions.Logging;

namespace Iwentys.GoogleTableIntegration.Marks
{
    public class MarkParser : ITableRequest<List<StudentSubjectScore>>
    {
        public string Id => _helper.Id;
        public string Range => _helper.Range;

        private readonly ILogger _logger;
        private readonly TableStringHelper _helper;

        public MarkParser(GoogleTableData tableData, ILogger logger)
        {
            _logger = logger;
            _helper = new TableStringHelper(tableData);
        }

        public List<StudentSubjectScore> Parse(ValueRange values)
        {
            var result = new List<StudentSubjectScore>();
            foreach (IList<object> row in values.Values)
            {
                var name = row[_helper.NameColumnNum];
                var score = row[_helper.ScoreColumnNum];
                if (name != null && score != null)
                {
                    string fullName = string.Join(" ", _helper.NameColumns.Select(c => row[c]));

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
    }
}