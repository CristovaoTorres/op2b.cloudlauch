using Platform.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Platform.Client.Extensions
{
    public static class UserExtensions
    {
        public static string FullName(this ClaimsPrincipal user)
        {
            var fullNameClaim = user.FindFirst("fullName");
            if (fullNameClaim == null)
            {
                return user.Identity.Name;
            }
           return fullNameClaim.Value;
        }

        public static string Role(this ClaimsPrincipal user)
        {
            return user.FindFirst("role")?.Value ?? "-";
        }


        public static string Email(this ClaimsPrincipal user)
        {
            return user.Identity.Name;
        }
        
    }
}
