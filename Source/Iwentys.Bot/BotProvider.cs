using Iwentys.Bot.ApiIntegration;
using Iwentys.Bot.Commands;
using Iwentys.Bot.Tools;
using Tef.BotFramework.Telegram;

namespace Iwentys.Bot
{
    public static class BotProvider
    {
        public static Tef.BotFramework.Core.Bot Init()
        {
            var identifier = new UserIdentifier();
            var apiProvider = new IwentysApiProvider();

            var telegramApiProvider = new TelegramApiProvider(new TelegramDebugSettings());
            var botInstance = new Tef.BotFramework.Core.Bot(telegramApiProvider)
                .AddCommand(new GetAllStudentsCommand(apiProvider.StudentApi))
                .AddCommand(new GetCurrentStudentCommand(apiProvider.StudentApi, identifier))
                .AddLogger();

            return botInstance;
        }
    }
}
