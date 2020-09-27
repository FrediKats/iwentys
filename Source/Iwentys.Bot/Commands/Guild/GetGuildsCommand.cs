using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Guild
{
    public class GetGuildsCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;

        public GetGuildsCommand(IwentysApiProvider iwentysApi)
        {
            _iwentysApi = iwentysApi;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            ICollection<GuildProfilePreviewDto> guildProfilePreviews = await _iwentysApi.Client.ApiGuildGetAsync(null, null).ConfigureAwait(false);

            return ResultFormatter.FormatAsListToResult(guildProfilePreviews);
        }

        public string CommandName { get; } = nameof(GetGuildsCommand);
        public string Description { get; } = nameof(GetGuildsCommand);
        public string[] Args { get; } = Array.Empty<string>();
    }
}