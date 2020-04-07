using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using react_app.AuthorizationRequirements;
using react_app.Data;
using System.Security.Claims;

namespace react_app
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
            services.AddDbContext<AppDbContext>(config => {
                config.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            services.AddAuthorization(congif => {
                // var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                // var defaultAuthPolicy = defaultAuthBuilder
                //     .RequireAuthenticatedUser()
                //     .RequireClaim(ClaimTypes.DateOfBirth)
                //     .Build();

                // congif.DefaultPolicy = defaultAuthPolicy;

                // congif.AddPolicy("Claim.DoB", policyBuilder =>{
                //     policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });

                congif.AddPolicy("Admin", policyBuilder => {
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
                });

                congif.AddPolicy("Claim.Dob", policyBuilder => {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });

                // congif.AddPolicy("Claim.DoB", policyBuilder =>
                // {
                //     policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                // });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
