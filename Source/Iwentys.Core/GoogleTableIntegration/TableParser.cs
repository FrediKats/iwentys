using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.GoogleTableIntegration
{
    public class TableParser
    {
        private readonly ILogger _logger;
        private readonly SheetsService _service;

        public TableParser(ILogger logger, SheetsService service)
        {
            _logger = logger;
            _service = service;
        }

        public T Execute<T>(ITableRequest<T> request)
        {
            _logger.LogTrace($"[TableParser] Parse table {request.Id}");
            SpreadsheetsResource.ValuesResource.GetRequest dataRequest = _service.Spreadsheets.Values.Get(request.Id, request.Range);
            ValueRange data = dataRequest.Execute();
            return request.Parse(data);
        }
    }
}
