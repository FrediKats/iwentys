using System;
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
    public class GetAllStudentsCommand : IBotCommand
    {
        private readonly StudentService _studentService;

        public GetAllStudentsCommand(ServiceProvider serviceProvider)
        {
            _studentService = serviceProvider.GetService<StudentService>();
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            StudentFullProfileDto[] profileDtos = _studentService.Get();
            return ResultFormatter.FormatToTask(profileDtos);
        }

        public string CommandName => nameof(GetAllStudentsCommand);
        public string Description => nameof(GetAllStudentsCommand);
        public string[] Args => Array.Empty<String>();
    }
}