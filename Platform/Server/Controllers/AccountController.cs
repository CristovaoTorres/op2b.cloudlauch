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
    public class AccountController : ControllerBase
    {

        private readonly IAccountService accountManagement;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountController(IAccountService accountManagement, IHttpContextAccessor httpContextAccessor)
        {
            this.accountManagement = accountManagement;
            this.httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Retorna todos os usuarios cadastrados.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpGet]
        public async Task<IEnumerable<UserModel>> Get()
        {
            return await accountManagement.GetAllUsersAsync();
        }

        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPut]
        public async Task<ResponseBase> Update([FromBody] UserModel user)
        {
            return await accountManagement.UpdateAsync(user);
        }

        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPost]
        public async Task<ResponseBase<string>> AddUser([FromBody] UserModel user)
        {
            return await accountManagement.AddUserAsync(user);
        }

        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpDelete]
        public async Task<ResponseBase> DeleteUser(string id)
        {
            return await accountManagement.DeleteAsync(id);
        }

    }
}
