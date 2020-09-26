using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class GetGroupStudentsCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;

        public GetGroupStudentsCommand(IwentysApiProvider iwentysApi)
        {
            _iwentysApi = iwentysApi;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            //TODO: move to extensions
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            IEnumerable<StudentFullProfileDto> profileDtos = await _iwentysApi.Student.Get(args.Arguments[0]).ConfigureAwait(false);

            return Result.Ok($"Group list {args.Arguments[0]}\n{ResultFormatter.FormatAsList(profileDtos)}");
        }

        public string CommandName => nameof(GetGroupStudentsCommand);
        public string Description => nameof(GetGroupStudentsCommand);
        public string[] Args => new[] {"group name"};
    }
}