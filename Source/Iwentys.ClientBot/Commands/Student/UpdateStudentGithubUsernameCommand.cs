using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Students;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class UpdateStudentGithubUsernameCommand : IBotCommand
    {
        private readonly IStudentService _studentService;
        private readonly UserIdentifier _userIdentifier;

        public UpdateStudentGithubUsernameCommand(ServiceProvider serviceProvider, UserIdentifier userIdentifier)
        {
            _studentService = serviceProvider.GetService<IStudentService>();
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser user = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentFullProfileDto profile = _studentService.AddGithubUsername(user.Id, args.Arguments[0]);
            return Task.FromResult(Result.Ok(profile.FormatFullInfo()));
        }

        public string CommandName { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string Description { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string[] Args { get; } = {"New github username"};
    }
}