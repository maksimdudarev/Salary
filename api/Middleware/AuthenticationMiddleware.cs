using MD.Salary.WebApi.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly object AuthenticationMiddlewareKey = new object();

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IEmployeeRepository _repository)
        {
            var header = "Authorization";

            if (!httpContext.Request.Headers.Keys.Contains(header))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            else
            {
                var value = httpContext.Request.Headers[header].ToString().Split(null).Last();
                var item = await _repository.GetTokenByValueAsync(value);
                if (item == null)
                {
                    httpContext.Response.StatusCode = 
                        httpContext.Request.Path.ToString() == "/api/authentication/logout" ? 
                        (int)HttpStatusCode.NotFound : (int)HttpStatusCode.Unauthorized;
                    return;
                }
                httpContext.Items[AuthenticationMiddlewareKey] = item;
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
