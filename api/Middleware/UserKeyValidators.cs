using ContactsApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ContactsApi.Middleware
{
    public class UserKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        private IContactsRepository Repo { get; set; }

        public UserKeyValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IContactsRepository _repo)
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
        }

    }

    public static class UserKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyUserKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserKeyValidatorsMiddleware>();
            return app;
        }
    }
}
