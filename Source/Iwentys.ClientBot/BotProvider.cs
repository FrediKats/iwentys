using Iwentys.ClientBot.Commands.Guild;
using Iwentys.ClientBot.Commands.Student;
using Iwentys.ClientBot.Commands.StudentLeaderboard;
using Iwentys.ClientBot.Commands.Tools;
using Iwentys.ClientBot.Tools;
using Iwentys.Core;
using Iwentys.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Tef.BotFramework.Core;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.ClientBot
{
    public static class BotProvider
    {
        public static Bot Init(IGetSettings<TelegramSettings> settings, ILogger logger)
        {
            var serviceCollection = new ServiceCollection();
            ServiceDiManager.RegisterAbstractionsImplementation(serviceCollection, ApplicationOptions.GithubToken);
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
