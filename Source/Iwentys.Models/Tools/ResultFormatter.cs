using System.Collections.Generic;
using System.Linq;
using FluentResults;

namespace Iwentys.Models.Tools
{
    public static class ResultFormatter
    {
        public static Result<string> Format(IEnumerable<IResultFormat> data)
        {
            return Result.Ok(string.Join("\n", data.Select(c => c.Format())));
        }

        public static Result<string> FormatAsList(IEnumerable<IResultFormat> data)
        {
            return Result.Ok(string.Join("\n", data.Select((c, i) => $"{i + 1}. {c.Format()}")));
        }
    }
}