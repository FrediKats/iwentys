﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class GetAllStudentsCommand : IBotCommand
    {
        private readonly IIwentysStudentApi _iwentysStudentApi;

        public GetAllStudentsCommand(IIwentysStudentApi iwentysStudentApi)
        {
            _iwentysStudentApi = iwentysStudentApi;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            IEnumerable<StudentFullProfileDto> profileDtos = await _iwentysStudentApi.Get();
            return ResultFormatter.Format(profileDtos);
        }

        public string CommandName => nameof(GetAllStudentsCommand);
        public string Description => nameof(GetAllStudentsCommand);
        public string[] Args => Array.Empty<String>();
    }
}