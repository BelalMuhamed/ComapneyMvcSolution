using CompaneyMvcBLL.Interfaces;
using CompaneyMvcBLL.Repositries;
using CompaneyMvcDAL.DbContext;
using CompaneyMvcDAL.Models;
using CompaneyMvcPL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComapneyMvc
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
            services.AddControllersWithViews();
            services.AddDbContext<ComapneyMvcDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")) );  
            //services.AddScoped<IDepartmentRepositry,DepartmentRepositry>();
            //services.AddScoped<IEmployeeRepositry,EmployeeRepositry>();
            
            services.AddAutoMapper(M => { M.AddProfile(new EmployeeProfile()); M.AddProfile(new DepartmentProfile());M.AddProfile(new UserProfile());M.AddProfile(new RoleProfile()); });

            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddIdentity<ApplicationUser, IdentityRole>(Options => 
            {
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true; }).AddEntityFrameworkStores<ComapneyMvcDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                Options.LoginPath=("_Account/Login");
                Options.AccessDeniedPath = "_Account/AccessDenied";
            });
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
                    pattern: "{controller=_Account}/{action=Login}/{id?}");
            });
        }
    }
}
