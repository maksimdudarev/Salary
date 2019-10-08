using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MD.Salary.WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private IUserRepository Repo { get; set; }

        public AuthenticationMiddleware(RequestDelegate next, IUserRepository _repo)
        {
            _next = next;
            Repo = _repo;
        }

        public async Task Invoke(HttpContext httpContext)
        {
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
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
