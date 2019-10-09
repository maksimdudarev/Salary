using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MD.Salary.WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private IUserRepository Repo { get; set; }

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserRepository _repo)
        {
            Repo = _repo;

            if (!httpContext.Request.Headers.Keys.Contains("user-key"))
            {
                httpContext.Response.StatusCode = 400; //Bad Request                
                await httpContext.Response.WriteAsync("User Key is missing");
                return;
            }
            else
            {
                if (!Repo.CheckValidUserKey(httpContext.Request.Headers["user-key"]))
                {
                    httpContext.Response.StatusCode = 401; //UnAuthorized
                    await httpContext.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }

            await _next.Invoke(httpContext);
            return;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
