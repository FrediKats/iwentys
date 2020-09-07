﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.ClientBot.ApiIntegration;
using Iwentys.ClientBot.Tools;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Common;

namespace Iwentys.ClientBot.Commands.Student
{
    public class GetAllStudentsCommand : IBotCommand
    {
        private readonly IIwentysStudentApi _iwentysStudentApi;

        public GetAllStudentsCommand(IIwentysStudentApi iwentysStudentApi)
        {
            _iwentysStudentApi = iwentysStudentApi;
        }

        public bool CanExecute(CommandArgumentContainer args)
        {
            return true;
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            throw new NotImplementedException();
        }

        public Result Execute(CommandArgumentContainer args)
        {
            IEnumerable<StudentFullProfileDto> profileDtos = _iwentysStudentApi.Get().Result;
            return ResultHelper.Of(profileDtos);
        }

        public string CommandName => nameof(GetAllStudentsCommand);
        public string Description => nameof(GetAllStudentsCommand);
        public string[] Args => Array.Empty<String>();
    }
}