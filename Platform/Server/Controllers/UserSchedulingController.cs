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
using Platform.Server.Services;
using Platform.Shared;
using Platform.Shared.Models;

namespace Platform.Server.Controllers
{


    /// <summary>
    /// Controla as informacoes de agendamento da VMs pelos usuarios. Start/Stop de VMs em dia da semana e horas especificas.
    /// </summary>
   [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserSchedulingController : ControllerBase
    {

        private readonly IUserVMSchedulingService userSchedulingService;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserSchedulingController(IUserVMSchedulingService userSchedulingService,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.userSchedulingService = userSchedulingService;
            this.httpContextAccessor = httpContextAccessor;
        
        }



        /// <summary>
        /// Retorna todas as informacoes de agendamento que o usuario informado possui. 
        /// ATENCAO: Somente Administradores podem "pedir" informacoes em nome de outros usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseBase<IList<UserVMScheduling>>> Get([FromQuery] string userId)
        {
            return await userSchedulingService.GetAsync(this.User, userId);
        }



        /// <summary>
        /// Adiciona as informacoes de agendamentos ao usuario informado.
        /// ATENCAO: Somente Administradores podem "adicionar" informacoes em nome de outros usuarios.
        /// </summary>
        /// <param name="userId">Id do usuario afetado</param>
        /// <param name="items">itens que deseja adicionar</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseBase> AddAsync([FromQuery] string userId, [FromBody] IList<UserVMScheduling> items)
        {
            return await userSchedulingService.AddAsync(this.User, userId, items);
        }



    }
}
