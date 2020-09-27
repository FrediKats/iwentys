using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
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

        public static TableParser Create(ILogger logger)
        {
            return new TableParser(logger, GetServiceForApiToken());
        }

        public T Execute<T>(ITableRequest<T> request)
        {
            _logger.LogTrace($"[TableParser] Parse table {request.Id}");
            SpreadsheetsResource.ValuesResource.GetRequest dataRequest = _service.Spreadsheets.Values.Get(request.Id, request.Range);
            ValueRange data = dataRequest.Execute();
            return request.Parse(data);
        }

        private static SheetsService GetServiceForCredential()
        {
            GoogleCredential credential = GoogleCredential
                .FromJson(ApplicationOptions.GoogleServiceToken)
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "IwentysTableParser",
                HttpClientInitializer = credential,
            });
        }

        private static SheetsService GetServiceForApiToken()
        {
            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "IwentysTableParser",
                ApiKey = ApplicationOptions.GoogleServiceToken
            });
        }
    }
}
