using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Iwentys.Infrastructure.Application.Middlewares
{
    public class ModelStateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState.GetErrorsString());
            else
                await next();
        }
    }
}