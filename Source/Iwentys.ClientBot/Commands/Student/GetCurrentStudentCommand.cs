using System;
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
    public class GetCurrentStudentCommand : IBotCommand
    {
        private readonly IStudentService _studentService;
        private readonly UserIdentifier _userIdentifier;

        public GetCurrentStudentCommand(ServiceProvider serviceProvider, UserIdentifier userIdentifier)
        {
            _studentService = serviceProvider.GetService<IStudentService>();
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            if (currentUser is null)
                return Result.Fail("Current user is not set");

            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentFullProfileDto profile = _studentService.Get(currentUser.Id);
            return Task.FromResult(Result.Ok(profile.FormatFullInfo()));
        }

        public string CommandName => nameof(GetCurrentStudentCommand);
        public string Description => nameof(GetCurrentStudentCommand);
        public string[] Args => Array.Empty<string>();
    }
}