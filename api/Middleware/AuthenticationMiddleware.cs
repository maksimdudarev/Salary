using MD.Salary.WebApi.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserRepository repo)
        {
            var header = "Authorization";

            if (!httpContext.Request.Headers.Keys.Contains(header))
            {
                httpContext.Response.StatusCode = 400; //Bad Request                
                await httpContext.Response.WriteAsync("Token is missing");
                return;
            }
            else
            {
                var value = httpContext.Request.Headers[header].ToString().Split(null).Last();
                var item = await repo.GetTokenByValueAsync(value);
                if (item != null)
                {
                    httpContext.Response.StatusCode = 401; //UnAuthorized
                    await httpContext.Response.WriteAsync("Invalid Token");
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
