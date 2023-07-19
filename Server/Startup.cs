using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Platform.Server.Data;
using Platform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Platform.Server.ActionFilters;
using Platform.Shared;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using Platform.Server.Services;
using System;
using Microsoft.Extensions.Logging;
using Platform.Shared.Extensions;
using Platform.Client.Strategies;

namespace Platform.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHttpContextAccessor();
            
          //  services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, Client.Services.CustomAuthenticationStateProvider>();
           
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            
            services.AddScoped<IUserVMSchedulingService, UserVMSchedulingService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IVirtualMachineService, VirtualMachineService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserClaimsService, UserClaimsService>();
            //services.AddScoped<IStrategyCloud>(provider =>
            //{
            //    var selectedStrategy = Configuration.GetValue<string>("SelectedStrategy");

            //    if (selectedStrategy == "AZURE")
            //    {
            //        return provider.GetService<StrategyAZURE>();
            //    }
            //    else if (selectedStrategy == "AWS")
            //    {
            //        return provider.GetService<StrategyAWS>();
            //    }

            //    // Retorne uma implementação padrão ou lance uma exceção, caso necessário
            //    throw new Exception("Estratégia não encontrada");
            //});

            var identityBuilder = services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                    {
                        var allClaims = ClaimsPermissionsExtensions.GetAllClaims();
                        for (int i = 0; i < allClaims.Length; i++)
                        {
                            var claim = allClaims[i];
                            options.IdentityResources["profile"].UserClaims.Add(claim.Type);
                            options.ApiResources.Single().UserClaims.Add(claim.Type);
                        }

                        options.IdentityResources["profile"].UserClaims.Add("fullName");

                        options.IdentityResources["openid"].UserClaims.Add("role");
                        options.ApiResources.Single().UserClaims.Add("role");

                        options.IdentityResources["openid"].UserClaims.Add(ClaimsConstants.CLAIM_FULL_NAME);
                        options.ApiResources.Single().UserClaims.Add(ClaimsConstants.CLAIM_FULL_NAME);

                        options.IdentityResources["openid"].UserClaims.Add(JwtClaimTypes.Email);
                        options.ApiResources.Single().UserClaims.Add(JwtClaimTypes.Email);
                    });

           // Need to do this as it maps "role" to ClaimTypes.Role and causes issues
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrators", policy => policy.RequireClaim("IsAdmin"));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
            });

          //  services.AddTransient<DataSeeder>();
            services.AddScoped<UserContextActionFilter>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove(JwtClaimTypes.Role);
            services.AddAuthentication().AddIdentityServerJwt();

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    builder.AllowAnyOrigin());// ("https://op2b-vm-test.azurewebsites.net"));
            //});

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();

            //app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            logger.LogInformation("Rodando Migrate...");
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    logger.LogError($"Erro ao fazer Migrate: {ex.ToString()}", ex);
                    throw;
                }

            }
        }
    }
}
