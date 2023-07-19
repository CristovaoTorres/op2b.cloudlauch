using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Platform.Shared.Extensions;
using Platform.Shared.Models;

namespace Platform.Server.ActionFilters
{
    public class UserContextActionFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {


            var userContext = new UserContextInfo(context.HttpContext.User.Identity, context.HttpContext.Connection.GetIpAddress());
            if (userContext.Headers == null)
            {
                userContext.Headers = new Dictionary<string, string>();
            }

            foreach (var header in context.HttpContext.Request.Headers)
            {
                userContext.Headers.Add(header.Key, header.Value);
            }

            context.HttpContext.Items["UserContext"] = userContext;

            var resultContext = await next();

        }
    }
}
