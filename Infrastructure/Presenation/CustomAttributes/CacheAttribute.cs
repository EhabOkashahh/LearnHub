using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstraction;

namespace Presenation.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute(int duration) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var key = BuildCachingKey(context.HttpContext);

            var cached = await cacheService.GetAsync(key);
            if (!string.IsNullOrEmpty(cached))
            {
                context.Result = new ContentResult
                {
                    Content = cached,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            var ActionContext = await next.Invoke();
            if(ActionContext.Result is OkObjectResult ok)
            {
                var json = JsonSerializer.Serialize(ok.Value, ok.Value.GetType());
                await cacheService.SetAsync(key, json, TimeSpan.FromSeconds(duration));

            }
        }

        private static string BuildCachingKey(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.ToString();
            var sortedQueryParams = string.Join("&", httpContext.Request.Query
                                            .OrderBy(x => x.Key)
                                            .Select(x => $"{x.Key}={x.Value}"));

            return string.IsNullOrWhiteSpace(sortedQueryParams) ? path : $"{path}?{sortedQueryParams}";
        }
    }
}