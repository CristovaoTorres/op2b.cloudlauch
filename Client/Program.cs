using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Client.Services;
using Platform.Client.Models;
using Platform.Client.Strategies;


namespace Platform.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder, builder.Services);

            var host = builder.Build();

            var serviceProvider = host.Services;


            await builder.Build().RunAsync();
        }


        public static void ConfigureServices(WebAssemblyHostBuilder builder, IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                var app = config.GetSection("App").Get<AppConfiguration>();
                return app;
            });


            //services.AddScoped<IClaimsTransformation, UserInfoClaims>();

            services.AddHttpClient("Platform.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Platform.ServerAPI"));

            services.AddHttpContextAccessor();


            services.AddScoped<ManageAccountsService>();
            services.AddScoped<SchedulingUIService>();
            services.AddScoped<VirtualMachineService>();

            services.AddScoped<StrategyAZURE>();
            services.AddScoped<StrategyAWS>();
            services.AddScoped<IStrategyCloudFactory, StrategyCloudFactory>();

            // Registrar o CloudContext
            services.AddScoped<CloudContext>();

            services.AddAuthorizationCore();
          
            services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();


        }
    }
}

