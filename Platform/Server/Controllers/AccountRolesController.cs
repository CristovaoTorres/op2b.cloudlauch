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

  
    [ApiController]
    [Route("[controller]")]
    public class AccountRolesController : ControllerBase
    {
        private readonly IRolesService rolesService;

        public AccountRolesController(IRolesService  rolesService)
        {
            this.rolesService = rolesService;
        }


        /// <summary>
        /// Retorna todas as roles existentes.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpGet]
        public  IEnumerable<RoleModel> Get()
        {
            return rolesService.GetAllRoles();
        }

     

    }
}
