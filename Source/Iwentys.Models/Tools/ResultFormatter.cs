using System.Collections.Generic;
using System.Linq;
using FluentResults;

namespace Iwentys.Models.Tools
{
    public static class ResultFormatter
    {
        public static string Format(IEnumerable<IResultFormat> data)
        {
            return string.Join("\n", data.Select(c => c.Format()));
        }

        public static Result<string> FormatToResult(IEnumerable<IResultFormat> data)
        {
            return Result.Ok(Format(data));
        }

        public static string FormatAsList(IEnumerable<IResultFormat> data)
        {
            return string.Join("\n", data.Select((c, i) => $"{i + 1}. {c.Format()}"));
        }

        public static Result<string> FormatAsListToResult(IEnumerable<IResultFormat> data)
        {
            return Result.Ok(FormatAsList(data));
        }
    }
}