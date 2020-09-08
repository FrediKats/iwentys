using Iwentys.ClientBot.ApiSdk;
using Iwentys.ClientBot.Commands.Student;
using Iwentys.ClientBot.Tools;
using Serilog;
using Tef.BotFramework.Core;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.ClientBot
{
    public static class BotProvider
    {
        public static Bot Init(string apiHostUrl, IGetSettings<TelegramSettings> settings, ILogger logger)
        {
            var identifier = new UserIdentifier();
            var apiProvider = new IwentysApiProvider(apiHostUrl);

            var telegramApiProvider = new TelegramApiProvider(settings);
            Bot botInstance = new Bot(telegramApiProvider)
                .AddCommand(new GetAllStudentsCommand(apiProvider.StudentApi))
                .AddCommand(new GetCurrentStudentCommand(apiProvider.StudentApi, identifier))
                .AddLogger(logger)
                .SetPrefix('/');

            return botInstance;
        }
    }
}
