using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Core.Services;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.Endpoints.ClientBot.Commands.Student
{
    public class GetGroupStudentsCommand : IBotCommand
    {
        private readonly StudentService _studentService;

        public GetGroupStudentsCommand(ServiceProvider serviceProvider)
        {
            _studentService = serviceProvider.GetService<StudentService>();
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
            List<StudentFullProfileDto> profileDtos = await _studentService.GetAsync(args.Arguments[0]);

            return Result.Ok($"Group list {args.Arguments[0]}\n{ResultFormatter.FormatAsList(profileDtos)}");
        }

        public string CommandName => nameof(GetGroupStudentsCommand);
        public string Description => nameof(GetGroupStudentsCommand);
        public string[] Args => new[] {"group name"};
    }
}