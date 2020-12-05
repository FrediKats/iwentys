using System;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Iwentys.Features.Study;
using Iwentys.Integrations.GoogleTableIntegration;
using Iwentys.Integrations.GoogleTableIntegration.Marks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Iwentys.Polygon
{
    internal class TableParserExample
    {
        /// <summary>
        /// Пример того, как можно работать с существующим парсером таблиц.
        /// Чтобы все работало, необходимо создать файлик credentials.json,
        /// который можно сгенерировать тут - https://console.developers.google.com/apis
        /// </summary>
        /// Парсеру таблицы нужны следующие данные:
        /// idТаблицы НазваниеСтаницыНаТаблице
        /// ПерваяСтрочкаБлока ПоследняяСтрочкаБлока
        /// НазваниеСтолбцаСГруппой НазваниеСтолбцаСФИО НазваниеСтолбцаСБаллами
        /// 
        /// Все это пока что вводится с консоли. Вот примеры наборов значений:
        /// "1XcrYxQ-hoId1g2cH6t4Nh_e21ZLLkFfIqPS1JTc385M BARS/COMPENSATION 6 21 n A 2 B C O"
        /// "1-JbFg-6zZuNjrCA3bsawsS88e5kePKgA0mK9otQKrYk "семестр 2" 100 124 y M3105 1 A O"
        /// 
        /// Даже на них уже видны некоторые проблемы, например в первой таблице Ф и ИО раздельно,
        /// а во второй нет поля с группой
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            SheetsService sheetsService;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                var credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

                sheetsService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "IwentysTableParser",
                });
            }

            var logger = new Logger<TableParser>(new LoggerFactory());
            var parser = new TableParser(logger, sheetsService);

            var m3101 = parser.Execute(new MarkParser(M3101(), logger));
            var m3102 = parser.Execute(new MarkParser(M3102(), logger));
            var m3103 = parser.Execute(new MarkParser(M3103(), logger));
            var m3104 = parser.Execute(new MarkParser(M3104(), logger));

            Console.WriteLine(JsonConvert.SerializeObject(m3101.Concat(m3102).Concat(m3103).Concat(m3104)));
        }

        //TODO: do we need this?
        //private static GoogleTableData ReadFromConsole()
        //{
        //    Console.WriteLine("Id Таблицы:");
        //    string spreadSheetId = Console.ReadLine();
        //    Console.WriteLine("Название страницы:");
        //    var sheetName = Console.ReadLine();
        //    Console.WriteLine("Первая строка с данными в таблице:");
        //    var firstRow = Console.ReadLine();
        //    Console.WriteLine("Последняя строка с данными в таблице:");
        //    var lastRow = Console.ReadLine();

        //    Console.WriteLine("На сколько столбцов разбито ФИО:");
        //    var nameSplitNum = int.Parse(Console.ReadLine());
        //    string[] nameArr = new string[nameSplitNum];
        //    Console.WriteLine("Вводите столбцы с ФИО");
        //    for (var i = 0; i < nameSplitNum; i++)
        //    {
        //        nameArr[i] = Console.ReadLine();
        //    }
        //    Console.WriteLine("Столбец с баллами:");
        //    var scoreColumn = Console.ReadLine();
        //    GoogleTableData test = new GoogleTableData(spreadSheetId, sheetName, firstRow, lastRow, nameArr, scoreColumn);
        //    return test;
        //}

        private static GoogleTableData M3101()
        {
            return new GoogleTableData(
                "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                "M3101",
                "4",
                "24",
                new[] {"B"}, 
                "V");
        }

        private static GoogleTableData M3102()
        {
            return new GoogleTableData(
                "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                "M3102",
                "4",
                "25",
                new[] { "B" },
                "V");
        }

        private static GoogleTableData M3103()
        {
            return new GoogleTableData(
                "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                "M3103",
                "4",
                "25",
                new[] { "B" },
                "V");
        }

        private static GoogleTableData M3104()
        {
            return new GoogleTableData(
                "1BMRHimS4Ioo5cWX1yZdHFsSyViR_J2h8rhL8Wl_x3og",
                "M3104",
                "4",
                "25",
                new[] { "B" },
                "V");
        }
    }
}
