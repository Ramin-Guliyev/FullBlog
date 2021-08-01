using BlogApplication.Data;
using BlogApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/ramin";
               
                options.SlidingExpiration = true; 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".Ramin.Guliyev.Blog.Cookie",
                    SameSite = SameSiteMode.Strict

                };
            });


            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMessageService, MessageService>(x =>
                new MessageService(
                        Configuration["EmailSender:Host"],
                        Configuration.GetValue<int>("EmailSender:Port"),
                        Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                        Configuration["EmailSender:UserName"],
                        Configuration["EmailSender:Password"])
            );
            //services.AddScoped<IMessageService, Smtp2GoMessageService>();
            services.AddControllersWithViews();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();  //https://aka.ms/aspnetcore-hsts read more about this 
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
