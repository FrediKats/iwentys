using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.ClientBot.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class UpdateStudentGithubUsernameCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;
        private readonly UserIdentifier _userIdentifier;

        public UpdateStudentGithubUsernameCommand(IwentysApiProvider iwentysApi, UserIdentifier userIdentifier)
        {
            _iwentysApi = iwentysApi;
            _userIdentifier = userIdentifier;
        }

        public bool CanExecute(CommandArgumentContainer args)
        {
            return true;
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            string token = await _iwentysApi.DebugCommand.LoginOrCreate(currentUser.Id);
            StudentFullProfileDto profile = await _iwentysApi.StudentApi.Update(new StudentUpdateDto { GithubUsername = args.Arguments[0]}, token);
            return Result.Ok(profile.FormatFullInfo());
        }

        public string CommandName { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string Description { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string[] Args { get; } = {"New github username"};
    }
}