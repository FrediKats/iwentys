using Iwentys.Endpoints.ClientBot.Commands.Guild;
using Iwentys.Endpoints.ClientBot.Commands.Student;
using Iwentys.Endpoints.ClientBot.Commands.StudentLeaderboard;
using Iwentys.Endpoints.ClientBot.Commands.Tools;
using Iwentys.Endpoints.ClientBot.Tools;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Tef.BotFramework.Core;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.Endpoints.ClientBot
{
    public static class BotProvider
    {
        public static Bot Init(IGetSettings<TelegramSettings> settings, ILogger logger, ServiceCollection serviceCollection)
        {
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var identifier = new UserIdentifier();

            var telegramApiProvider = new TelegramApiProvider(settings);
            Bot botInstance = new Bot(telegramApiProvider)
                .AddCommand(new GetAllStudentsCommand(serviceProvider))
                .AddCommand(new GetCurrentStudentCommand(serviceProvider, identifier))
                .AddCommand(new UpdateStudentGithubUsernameCommand(serviceProvider, identifier))
                .AddCommand(new GetStudentsRatingCommand(serviceProvider))
                .AddCommand(new GetGuildsCommand(serviceProvider))
                .AddCommand(new GetGroupStudentsCommand(serviceProvider))
                .AddLogger(logger)
                .SetPrefix('/');

            //TODO: debug methods
            botInstance.AddCommand(new SetCurrentUserCommand(identifier));

            return botInstance;
        }
    }
}
