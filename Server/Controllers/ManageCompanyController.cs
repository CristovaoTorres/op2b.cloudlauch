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
    public class ManageCompanyController : ControllerBase
    {

        private readonly ICompanyService companyService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ManageCompanyController(ICompanyService companyService, IHttpContextAccessor httpContextAccessor)
        {
            this.companyService = companyService;
            this.httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// Retorna todas as empresas (clientes) cadastradas.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpGet]
        public async Task<IEnumerable<CompanyModel>> Get([FromQuery] int[] ids = null)
        {
            return await companyService.GetAsync(ids);
        }



        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPut]
        public async Task<ResponseBase> Update([FromBody] CompanyModel target)
        {
            return await companyService.UpdateAsync(target);
        }


        /// <summary>
        /// Adiciona uma nova empresa
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpPost]
        public async Task<ResponseBase<int>> AddCompany([FromBody] CompanyModel target)
        {
            return await companyService.AddAsync(target);
        }


        [Authorize(Roles = ROLES.ADMINISTRATOR_NAME)]
        [HttpDelete]
        public async Task<ResponseBase> DeleteUser(int id)
        {
            return await companyService.DeleteAsync(id);
        }

    }
}
