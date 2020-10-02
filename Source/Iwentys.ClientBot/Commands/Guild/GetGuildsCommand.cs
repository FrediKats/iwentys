using System;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Guild
{
    public class GetGuildsCommand : IBotCommand
    {
        private readonly IGuildService _guildService;

        public GetGuildsCommand(ServiceProvider serviceProvider)
        {
            _guildService = serviceProvider.GetService<IGuildService>();
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            GuildProfilePreviewDto[] guildProfilePreviews = _guildService.GetOverview(0, 20);

            return ResultFormatter.FormatAsListToTask(guildProfilePreviews);
        }

        public string CommandName { get; } = nameof(GetGuildsCommand);
        public string Description { get; } = nameof(GetGuildsCommand);
        public string[] Args { get; } = Array.Empty<string>();
    }
}