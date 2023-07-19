using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Platform.Client.Services
{
    public class RolesClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly IServiceProvider services;

        public RolesClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor, IServiceProvider services) : base(accessor)
        {
            this.services = services;
        }
        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(  RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {

            //var localStore = services.GetRequiredService<LocalStore>();

            //var user = await base.CreateUserAsync(account, options);
            //if (user.Identity.IsAuthenticated)
            //{
            //    var identity = (ClaimsIdentity)user.Identity;
            //    var roleClaims = identity.FindAll(identity.RoleClaimType);
            //    if (roleClaims != null && roleClaims.Any())
            //    {
            //        foreach (var existingClaim in roleClaims.ToList())
            //        {
            //            identity.RemoveClaim(existingClaim);
            //        }
            //        var rolesElem = account.AdditionalProperties[identity.RoleClaimType];
            //        if (rolesElem is JsonElement roles)
            //        {
            //            if (roles.ValueKind == JsonValueKind.Array)
            //            {
            //                foreach (var role in roles.EnumerateArray())
            //                {
            //                    identity.AddClaim(new Claim(options.RoleClaim, role.GetString()));
            //                }
            //            }
            //            else
            //            {
            //                identity.AddClaim(new Claim(options.RoleClaim, roles.GetString()));
            //            }
            //        }
            //    }

            //    await localStore.SaveUserAccountAsync(user);
            //}
            //else
            //{
            //    user = await localStore.LoadUserAccountAsync();
            //}

            //return user;




            var user = await base.CreateUserAsync(account, options);
            if (user.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user.Identity;
                var roleClaims = identity.FindAll(identity.RoleClaimType);
                if (roleClaims != null && roleClaims.Any())
                {
                    foreach (var existingClaim in roleClaims.ToList())
                    {
                        identity.RemoveClaim(existingClaim);
                    }
                    var rolesElem = account.AdditionalProperties[identity.RoleClaimType];
                    if (rolesElem is JsonElement roles)
                    {
                        if (roles.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var role in roles.EnumerateArray())
                            {
                                identity.AddClaim(new Claim(options.RoleClaim, role.GetString()));
                            }
                        }
                        else
                        {
                            identity.AddClaim(new Claim(options.RoleClaim, roles.GetString()));
                        }
                    }
                }
            }
            return user;
        }
    }
}
