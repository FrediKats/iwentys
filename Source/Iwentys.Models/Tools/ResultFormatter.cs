using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Models.Tools
{
    public static class ResultFormatter
    {
        public static string Format(IEnumerable<IResultFormat> data)
        {
            return string.Join("\n", data.Select(c => c.Format()));
        }
    }
}