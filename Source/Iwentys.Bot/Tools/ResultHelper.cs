﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Tools;
using Tef.BotFramework.Common;

namespace Iwentys.ClientBot.Tools
{
    public static class ResultHelper
    {
        public static Result<string> Of(IResultFormat format)
        {
            return Result<string>.Ok(format.Format());
        }

        public static Result<string> Of<T>(IEnumerable<T> format) where T : IResultFormat
        {
            return Result<string>.Ok(string.Join("\n", format.Select(f => f.Format())));
        }
    }
}