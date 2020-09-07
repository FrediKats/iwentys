using Iwentys.ClientBot.ApiIntegration;
using Iwentys.ClientBot.Commands.Student;
using Iwentys.ClientBot.Tools;
using Serilog;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.ClientBot
{
    public static class BotProvider
    {
        public static Tef.BotFramework.Core.Bot Init(
            string apiHostUrl,
            IGetSettings<TelegramSettings> settings,
            ILogger logger)
        {
            var identifier = new UserIdentifier();
            var apiProvider = new IwentysApiProvider(apiHostUrl);

            //TODO: need to user logger, wait next BotFrame release
            var telegramApiProvider = new TelegramApiProvider(settings);
            var botInstance = new Tef.BotFramework.Core.Bot(telegramApiProvider)
                .AddCommand(new GetAllStudentsCommand(apiProvider.StudentApi))
                .AddCommand(new GetCurrentStudentCommand(apiProvider.StudentApi, identifier))
                .AddLogger(logger);

            return botInstance;
        }
    }
}
