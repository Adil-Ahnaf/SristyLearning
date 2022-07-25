using AspNetCore.Identity.Dapper;
using AspNetCore.Identity.Dapper.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SristyLearning.Data.SqlDb;
using SristyLearning.Helpers;
using System;

namespace SristyLearning
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
            
            services.AddIdentity<ApplicationUser, ApplicationRole>(
             options =>
             {
                 options.Password.RequiredLength = 8;
                 options.Password.RequireLowercase = true;
                 options.Password.RequireNonAlphanumeric = true;

                 options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

                 options.Lockout.MaxFailedAccessAttempts = 5;
                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                 options.User.RequireUniqueEmail = true;
                 options.SignIn.RequireConfirmedEmail = true;

             }
            )
            .AddDapperStores(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("SristyLearningDBConnection");
                options.DbSchema = "dbo";
            }).AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.Configure<SmtpSetting>(Configuration.GetSection("SMTP"));
            services.AddSingleton(new DbConnectionInfo { Key = "SristyLearningDBConnection" });

            services.AddSingleton<IEmailHelper, EmailHelper>();
            services.AddSingleton<IDataAccess, SqlDapperDataAccess>();
            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
