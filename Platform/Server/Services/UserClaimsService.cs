using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Platform.Server.Data;
using Platform.Shared.Models;
using Platform.Shared.Extensions;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    /// <summary>
    /// Servico para controlar os claims que cada usuario possui.
    /// </summary>
    public class UserClaimsService : IUserClaimsService
    {
        private readonly ILogger<UserClaimsService> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserClaimsService(ILogger<UserClaimsService> logger,
            ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }




        /// <summary>
        /// Retorna todos os Claims ligados a permisso de acesso que o usuario possui.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<AppClaimModel>> GetAsync(ClaimsPrincipal caller, string userId)
        {
            var response = new ResponseBase();

            bool isAllowed = caller.IsAllowed(userId);

            if (!isAllowed)
            {
               // response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Denied", ErrorMessage = "Permissao de acesso negada" });
                return new List<AppClaimModel>();
            }

            var user = await userManager.FindByIdAsync(userId);

            //Obtem todos os claims do usuario, mesmos os que nao tem nenhuma relacao com permissao de acesso.
            var userClaims = await userManager.GetClaimsAsync(user);

            //obtem todos os claims de permissao de acesso que o App possui.
            var allAppPermissionClaims = ClaimsPermissionsExtensions.GetAllClaims();

            var returnList = new List<AppClaimModel>();

            //filtra somente os Claims de permissao de acesso.
            foreach (var userClaim in userClaims)
            {
                //pega somente os claims de permissao
                var permissionClaim = allAppPermissionClaims.FirstOrDefault(f => f.Type.Equals(userClaim.Type, System.StringComparison.OrdinalIgnoreCase));

                if (permissionClaim != null)
                {
                    permissionClaim.Value = userClaim.Value;
                    returnList.Add(permissionClaim);
                }
            }

            return returnList;
        }

        public async Task<ResponseBase> AddAsync(ClaimsPrincipal caller, string userId, IList<AppClaimModel> claims, bool removePreviousClaims = true)
        {
            var response = new ResponseBase();

            bool isAllowed = caller.IsAllowed(userId);

            if (!isAllowed)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Denied", ErrorMessage = "Permissao de acesso negada" });
                return response;
            }

            var user = await userManager.FindByIdAsync(userId);

            var errors = new Dictionary<string, string>();

            if (removePreviousClaims)
            {
                var existingClaims = await GetAsync(caller, userId);
                if (existingClaims.Any())
                {
                    await RemoveAsync(caller, userId, existingClaims);
                }
            }

            var result = await userManager.AddClaimsAsync(user, claims.ToList().Select(permissionClaim => new Claim(permissionClaim.Type, permissionClaim.Value)));

            if (result == null)
            {
                response.Success = false;
                errors.Add($"Null", $"Adicao de claims ao usuario \"{userId}\" retornou nulo");
                return response;
            }
            else if (!result.Succeeded)
            {
                response.Success = false;
                foreach (var error in result.Errors)
                {
                    errors.Add($"{error.Code}", error.Description);
                }
                return response;
            }


            response.Success = true;
            return response;
        }





        public async Task<ResponseBase> RemoveAsync(ClaimsPrincipal caller, string userId, IList<AppClaimModel> claims)
        {
            var response = new ResponseBase();

            var user = await userManager.FindByIdAsync(userId);

            bool isAllowed = caller.IsAllowed(userId);

            if (!isAllowed)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Denied", ErrorMessage = "Permissao de acesso negada" });
                return response;
            }

            var errors = new Dictionary<string, string>();

            var result = await userManager.RemoveClaimsAsync(user, claims.Select(permissionClaim => new Claim(permissionClaim.Type, permissionClaim.Value)));

            if (result == null)
            {
                errors.Add($"Null", $"Remocao de claims do usuario \"{userId}\" retornou nulo");
            }
            else if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add($"{ error.Code}", error.Description);
                }

            }


            return new ResponseBase { Success = !errors.Any(), Errors = errors };
        }
    }
}
