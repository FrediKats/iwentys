using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Tools;
using Result = Tef.BotFramework.Common.Result;

namespace Iwentys.Bot.Tools
{
    public static class ResultHelper
    {
        public static Result Of(IResultFormat format)
        {
            return new Result(true, format.Format());
        }

        public static Result Of<T>(IEnumerable<T> format) where T : IResultFormat
        {
            return new Result(true, string.Join("\n", format.Select(f => f.Format())));
        }
    }
}