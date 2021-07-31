using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Iwentys.Common.Tools
{
    public static class ModelStateExtensions
    {
        public static string GetErrorsString(this ModelStateDictionary modelState)
        {
            return String.Join("\n",modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)));
        }
    }
}