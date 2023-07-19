using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    public interface IVirtualMachineService
    {

        Task<ResponseBase<int>> AddAsync(VirtualMachineModel target);

        /// <summary>
        /// Retorna as VMs que a IdCompany informada possui.
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<VirtualMachineModel>> GetAsync(ClaimsPrincipal user, int? idCompanyNullable = null, int[] ids = null);
    

        Task<ResponseBase> UpdateAsync(VirtualMachineModel target);
        Task<ResponseBase> DeleteAsync(int id);
    }
}
