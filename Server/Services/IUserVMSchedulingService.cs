using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{

    /// <summary>
    /// Servico para controlar quando as VMs podem ser ligadas/desligadas pelos usuarios que possuirem os claims
    /// <see cref="Platform.Shared.ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM"/> e/ou 
    /// <see cref="Platform.Shared.ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM"/>
    /// </summary>
    public interface IUserVMSchedulingService
    {

        /// <summary>
        /// Adiciona a programacao de start/stop de VMs ao usuario informado.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="schedules"></param>
        /// <returns></returns>
        Task<ResponseBase> AddAsync(ClaimsPrincipal caller, string userId, IList<UserVMScheduling> schedules, bool removePrevious = true);


        /// <summary>
        /// Retorna a programacao de start/stop de VMs ao usuario informado.
        /// </summary>
        /// <param name="caller">dados do usuario-chamador que executa este metodo</param>
        /// <param name="userId">Id do usuario que deseja retornar os Schedules</param>
        /// <returns></returns>
        Task<ResponseBase<IList<UserVMScheduling>>> GetAsync(ClaimsPrincipal caller, string userId);

    }


}
