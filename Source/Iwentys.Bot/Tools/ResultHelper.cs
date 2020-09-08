using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Tools;
using Tef.BotFramework.Common;

namespace Iwentys.ClientBot.Tools
{
    //TODO: remove
    public static class ResultHelper
    {
        public static Result<string> Of(string str)
        {
            return Result<string>.Ok(str, str);
        }

        public static Result<string> Of(IResultFormat format)
        {
            return Result<string>.Ok(format.Format(), format.Format());
        }

        public static Result<string> Of<T>(IEnumerable<T> format) where T : IResultFormat
        {
            var str = string.Join("\n", format.Select(f => f.Format()));
            return Result<string>.Ok(str, str);
        }
    }
}