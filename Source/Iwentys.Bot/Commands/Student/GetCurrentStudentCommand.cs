using System;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ApiClient.OpenAPIService;
using Iwentys.ClientBot.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class GetCurrentStudentCommand : IBotCommand
    {
        private readonly IwentysApiProvider _api;
        private readonly UserIdentifier _userIdentifier;

        public GetCurrentStudentCommand(IwentysApiProvider api, UserIdentifier userIdentifier)
        {
            _api = api;
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            if (currentUser is null)
                return Result.Fail("Current user is not set");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            Client client = await _userIdentifier.GetProvider(args.Sender.UserSenderId, _api).ConfigureAwait(false);
            StudentFullProfileDto profile = await client.ApiStudentGetAsync(currentUser.Id).ConfigureAwait(false);
            return Result.Ok(profile.FormatFullInfo());
        }

        public string CommandName => nameof(GetCurrentStudentCommand);
        public string Description => nameof(GetCurrentStudentCommand);
        public string[] Args => Array.Empty<string>();
    }
}