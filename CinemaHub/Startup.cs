using CinemaHub.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaHub.Bootstrap;
using CinemaHub.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Moneyfold.API.Utility;

namespace CinemaHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Application.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CinemaHub API documentation",
                    Description = "This is the backend endpoints of CinemaHub",
                });
                options.OperationFilter<SwaggerFileOperationFilter>();
            });
           

            services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; });
            services.AddMvc();
            services.AddMvcCore(options =>
            {
                options.Filters.Add(new SecurityHeadersAttribute());
            }).AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.Value != null && context.Request.Path.HasValue && context.Request.Path.Value.Contains("api"))
                    {
                        context.Response.StatusCode = 401;
                       
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }

                    return Task.CompletedTask;
                };
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminOnly");

            });
            Seeder.RunDbMigrations(services).GetAwaiter().GetResult();

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddScssBundle("/lib/admin/scss/style.scss", "lib/admin/scss/style.scss");

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Seeder.SeedDatabase(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseWebOptimizer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Identity",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
