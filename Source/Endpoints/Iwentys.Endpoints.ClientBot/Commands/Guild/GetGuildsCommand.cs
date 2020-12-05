using System;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Services;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.Endpoints.ClientBot.Commands.Guild
{
    public class GetGuildsCommand : IBotCommand
    {
        private readonly GuildService _guildService;

        public GetGuildsCommand(ServiceProvider serviceProvider)
        {
            _guildService = serviceProvider.GetService<GuildService>();
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