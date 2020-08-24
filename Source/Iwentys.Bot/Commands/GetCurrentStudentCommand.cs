using System;
using Iwentys.Bot.ApiIntegration;
using Iwentys.Bot.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Common;

namespace Iwentys.Bot.Commands
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

        public Result Execute(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentFullProfileDto profile = _iwentysStudentApi.Get(currentUser.Id);
            
            return ResultHelper.Of(profile);
        }

        public string CommandName => nameof(GetCurrentStudentCommand);
        public string Description => nameof(GetCurrentStudentCommand);
        public string[] Args => Array.Empty<string>();
    }
}