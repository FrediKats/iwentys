using System.Threading.Tasks;
using FluentResults;
using Iwentys.Endpoints.ClientBot.Tools;
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Models;
using Iwentys.Features.StudentFeature.Services;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.Endpoints.ClientBot.Commands.Student
{
    public class UpdateStudentGithubUsernameCommand : IBotCommand
    {
        private readonly StudentService _studentService;
        private readonly UserIdentifier _userIdentifier;

        public UpdateStudentGithubUsernameCommand(ServiceProvider serviceProvider, UserIdentifier userIdentifier)
        {
            _studentService = serviceProvider.GetService<StudentService>();
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser user = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentPartialProfileDto profile = await _studentService.AddGithubUsernameAsync(user.Id, args.Arguments[0]);
            return Result.Ok(profile.Format());
        }

        public string CommandName { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string Description { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string[] Args { get; } = {"New github username"};
    }
}