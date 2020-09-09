using Iwentys.ClientBot.ApiSdk;
using Iwentys.ClientBot.Commands.Student;
using Iwentys.ClientBot.Commands.StudentLeaderboard;
using Iwentys.ClientBot.Commands.Tools;
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
            var apiProvider = new IwentysApiProvider(apiHostUrl);
            var identifier = new UserIdentifier();

            var telegramApiProvider = new TelegramApiProvider(settings);
            Bot botInstance = new Bot(telegramApiProvider)
                .AddCommand(new GetAllStudentsCommand(apiProvider.StudentApi))
                .AddCommand(new GetCurrentStudentCommand(apiProvider.StudentApi, identifier))
                .AddCommand(new UpdateStudentGithubUsernameCommand(apiProvider, identifier))
                .AddCommand(new GetStudentsRatingCommand(apiProvider))
                .AddLogger(logger)
                .SetPrefix('/');

            //TODO: debug methods
            botInstance
                .AddCommand(new SetCurrentUserCommand(identifier));

            return botInstance;
        }
    }
}
