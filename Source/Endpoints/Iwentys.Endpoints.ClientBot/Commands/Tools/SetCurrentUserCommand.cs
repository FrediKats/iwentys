using System.Threading.Tasks;
using FluentResults;
using Iwentys.Endpoints.ClientBot.Tools;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.Endpoints.ClientBot.Commands.Tools
{
    public class SetCurrentUserCommand : IBotCommand
    {
        private readonly UserIdentifier _userIdentifier;

        public SetCurrentUserCommand(UserIdentifier userIdentifier)
        {
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            if (!int.TryParse(args.Arguments[0], out _))
                return Result.Fail("Argument must be int value (courseId)");

            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            _userIdentifier.SetUser(args.Sender.UserSenderId, int.Parse(args.Arguments[0]));
            return Task.FromResult(Result.Ok($"New student id set: {args.Arguments[0]}"));
        }

        public string CommandName { get; } = nameof(SetCurrentUserCommand);
        public string Description { get; } = nameof(SetCurrentUserCommand);
        public string[] Args { get; } = {"Student id"};
    }
}