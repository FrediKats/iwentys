using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

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
        /// "1XcrYxQ-hoId1g2cH6t4Nh_e21ZLLkFfIqPS1JTc385M BARS/COMPENSATION 6 21 A B O"
        /// "1-JbFg-6zZuNjrCA3bsawsS88e5kePKgA0mK9otQKrYk "семестр 2" 100 124 Q A O"
        ///
        /// Даже на них уже видны некоторые проблемы, например в первой таблице Ф и ИО раздельно,
        /// а во второй нет поля с группой
        /// 
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Id Таблицы:");
            string spreadSheetId = Console.ReadLine();

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

            Console.WriteLine("Название страницы:");
            var sheetName = Console.ReadLine();
            Console.WriteLine("Первая строка с данными в таблице:");
            var firstRow = int.Parse(Console.ReadLine());
            Console.WriteLine("Последняя строка с данными в таблице:");
            var lastRow = int.Parse(Console.ReadLine());
            Console.WriteLine("Столбец с группой:");
            var groupColumn = Console.ReadLine();
            Console.WriteLine("Столбец с ФИО:");
            var nameColumn = Console.ReadLine();
            Console.WriteLine("Столбец с баллами:");
            var scoreColumn = Console.ReadLine();

            var tableParser = new Core.GoogleTableParsing.TableParser(sheetsService, spreadSheetId, sheetName,
                firstRow, lastRow, groupColumn, nameColumn, scoreColumn);

            Console.WriteLine(tableParser.GetStudentsJson());
        }
    }
}
