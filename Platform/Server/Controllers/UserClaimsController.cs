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


namespace Platform.Server.Controllers
{

   [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
    [ApiController]
    [Route("[controller]")]
    public class UserClaimsController : ControllerBase
    {

        private readonly IUserClaimsService userClaimsService;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserClaimsController(IUserClaimsService userClaimsService,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.userClaimsService = userClaimsService;
            this.httpContextAccessor = httpContextAccessor;
        
        }



        /// <summary>
        /// Retorna todas as claims que o usuario informado possui.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<AppClaimModel>> Get([FromQuery] string userId)
        {
            return await userClaimsService.GetAsync(this.User, userId);
        }



        /// <summary>
        /// Adiciona as claims informadas ao usuario informado.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseBase> AddAsync([FromQuery] string userId, [FromBody] IList<AppClaimModel> claims)
        {
            return await userClaimsService.AddAsync(this.User, userId, claims);
        }


        /// <summary>
        /// Remove as clains informadas do usuario informado.
        /// </summary>
        /// <param name="userId">Id User do usuario que deseja remover as claims</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResponseBase> RemoveAsync(string userId, IList<AppClaimModel> claims)
        {
            return await userClaimsService.RemoveAsync(this.User, userId, claims);
        }

    }
}
