using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Platform.Shared.Models;
using Platform.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Platform.Server.Services
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
            this.userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("fullName", user.FullName ?? ""));
            identity.AddClaim(new Claim("id", user.Id ?? ""));



            var allClaims = await userManager.GetClaimsAsync(user);  //ClaimsPermissionsExtensions.GetAllClaims();
            for (int i = 0; i < allClaims.Count; i++)
            {
                var claim = allClaims[i];
                identity.AddClaim(new Claim(claim.Type, claim.Value));
            }

            return identity;
        }
    }
}
