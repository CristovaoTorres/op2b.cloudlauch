using System;
using System.Security.Claims;

namespace Platform.Shared.Extensions
{
    public static  class ClaimsSecurityExtensions
    {
        /// <summary>
        /// Determina se o usuario-chamador tem permissao para acessar informacoes as informacoes do <paramref name="userId"/> informado.
        /// </summary>
        /// <param name="callingUser"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool IsAllowed(this ClaimsPrincipal callingUser, string userId)
        {

            bool isAdmin = callingUser.IsInRole(ROLES.ADMINISTRATOR_NAME);

            if (isAdmin)
            {
                return true;

            }

            foreach (var claim in callingUser.Claims)
            {
                if (ClaimTypes.NameIdentifier.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
                {
                    var claimUserId = claim.Value;
                    return userId.Equals(claimUserId, System.StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
            


        }



    }
}
