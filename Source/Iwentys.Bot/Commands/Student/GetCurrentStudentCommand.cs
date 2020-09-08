using System;
using System.Threading.Tasks;
using Iwentys.ApiSdk;
using Iwentys.ClientBot.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Common;

namespace Iwentys.ClientBot.Commands.Student
{
    public class GetCurrentStudentCommand : IBotCommand
    {
        private readonly IIwentysStudentApi _iwentysStudentApi;
        private readonly UserIdentifier _userIdentifier;

        public GetCurrentStudentCommand(IIwentysStudentApi iwentysStudentApi, UserIdentifier userIdentifier)
        {
            _iwentysStudentApi = iwentysStudentApi;
            _userIdentifier = userIdentifier;
        }

        public bool CanExecute(CommandArgumentContainer args)
        {
            return true;
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentFullProfileDto profile = await _iwentysStudentApi.Get(currentUser.Id);
            return ResultHelper.Of(profile);
        }

        public string CommandName => nameof(GetCurrentStudentCommand);
        public string Description => nameof(GetCurrentStudentCommand);
        public string[] Args => Array.Empty<string>();
    }
}