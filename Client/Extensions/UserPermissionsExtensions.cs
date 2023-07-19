using Platform.Shared;
using Platform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Platform.Client.Extensions
{
    public static class UserPermissionsExtensions
    {
      

        /// <summary>
        /// Determina se o usuario tem o Claim "IsAdmin" com o value "true"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimsConstants.PERMISSION_CLAIM_IS_ADMIN);
            if (claim == null)
            {
                return false;
            }
            return claim.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAdmin(this IList<AppClaimModel> user)
        {
            var claim = user.FirstOrDefault(f => f.Type.Equals(ClaimsConstants.PERMISSION_CLAIM_IS_ADMIN, StringComparison.OrdinalIgnoreCase));
            if (claim == null)
            {
                return false;
            }
            return claim.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        //


        /// <summary>
        /// Determina se o usuario tem permissao pra startar maquinas virtuais.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanStartVirtualMachine(this ClaimsPrincipal user)
        {
            if (IsAdmin(user))
            {
                return true;
            }


            var claim = user.FindFirst(ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM);
            if (claim == null)
            {
                return false;
            }
            return claim.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determina se o usuario tem permissao pra Stopar maquinas virtuais.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanStoptVirtualMachine(this ClaimsPrincipal user)
        {
            if (IsAdmin(user))
            {
                return true;
            }


            var claim = user.FindFirst(ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM);
            if (claim == null)
            {
                return false;
            }
            return claim.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }


    }
}
