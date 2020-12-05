using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;
using Iwentys.Features.Study;
using Microsoft.Extensions.Logging;

namespace Iwentys.Integrations.GoogleTableIntegration.Marks
{
    public class MarkParser : ITableRequest<List<StudentSubjectScore>>
    {
        private readonly TableStringHelper _helper;

        private readonly ILogger _logger;

        public MarkParser(GoogleTableData tableData, ILogger logger)
        {
            _logger = logger;
            _helper = new TableStringHelper(tableData);
        }

        public string Id => _helper.Id;
        public string Range => _helper.Range;

        public List<StudentSubjectScore> Parse(ValueRange values)
        {
            var result = new List<StudentSubjectScore>();
            foreach (IList<object> row in values.Values)
            {
                object name = row[_helper.NameColumnNum];
                object score = row[_helper.ScoreColumnNum];
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