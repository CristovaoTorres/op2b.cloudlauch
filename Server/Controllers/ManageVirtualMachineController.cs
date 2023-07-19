using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Platform.Shared.Models;
using Platform.Server.Services;
using Platform.Shared;
using Platform.Shared.Models;

namespace Platform.Server.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class ManageVirtualMachine : ControllerBase
    {

        private readonly IVirtualMachineService virtualMachineService;
        private readonly IHttpContextAccessor httpContextAccessor;


        public ManageVirtualMachine(IVirtualMachineService virtualMachineService,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.virtualMachineService = virtualMachineService;
            this.httpContextAccessor = httpContextAccessor;
        
        }



        /// <summary>
        /// Retorna todas as empresas (clientes) cadastradas.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<VirtualMachineModel>> Get([FromQuery] int? idCompany = null, [FromQuery] int[] ids = null)
        {
            return await virtualMachineService.GetAsync(this.User, idCompany, ids);
        }


        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPut]
        public async Task<ResponseBase> Update([FromBody] VirtualMachineModel target)
        {
            return await virtualMachineService.UpdateAsync(target);
        }


        /// <summary>
        /// Adiciona uma nova empresa
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPost]
        public async Task<ResponseBase<int>> AddCompany([FromBody] VirtualMachineModel target)
        {
            return await virtualMachineService.AddAsync(target);
        }


        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpDelete]
        public async Task<ResponseBase> DeleteUser(int id)
        {
            return await virtualMachineService.DeleteAsync(id);
        }

    }
}
