using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{


    public interface IUserClaimsService
    {

        /// <summary>
        /// Adiciona os claims informados ao usuario informado.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<ResponseBase> AddAsync(ClaimsPrincipal caller, string userId, IList<AppClaimModel> claims, bool removePreviousClaims = true);


        /// <summary>
        /// Retorna todos os Claims que um usuario possui.
        /// </summary>
        /// <param name="caller">dados do usuario-chamador que executa este metodo</param>
        /// <param name="userId">Id do usuario que deseja retornar os Claims</param>
        /// <returns></returns>
        Task<IList<AppClaimModel>> GetAsync(ClaimsPrincipal caller, string userId);

        /// <summary>
        /// Remove os Claims informados do usuario informado
        /// </summary>
        /// <param name="caller">dados do usuario-chamador que executa este metodo</param>
        /// <param name="target"></param>
        /// <param name="userId">Id do usuario que deseja remover os Claims</param>
        /// <returns></returns>
        Task<ResponseBase> RemoveAsync(ClaimsPrincipal caller, string userId, IList<AppClaimModel> claims);

    
    }

}
