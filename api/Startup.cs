using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Infrastructure;
using MD.Salary.WebApi.Models;
using ContactsApi.Middleware;
using ContactsApi.Repository;
using MD.Salary.WebApi.Middleware;

namespace MD.Salary.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EmployeeContext>(opt =>
                opt.UseSqlite(Constants.ConnectionStringApi));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IEmployeeRepository, EFEmployeeRepository>();
            services.AddScoped<IUserRepository, EFUserRepository>();
            services.AddScoped<IContactsRepository, EFContactsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            /*app.UseMyMiddleware();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });*/

            //app.ApplyUserKeyValidation();
            //app.UseAuthenticationMiddleware();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
