using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Server.Data;
using Platform.Shared.Models;
using Platform.Shared;
using Platform.Shared.Extensions;

namespace Platform.Server.Services
{

    public class VirtualMachineService : IVirtualMachineService
    {
        private readonly ILogger<VirtualMachineService> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public VirtualMachineService(ILogger<VirtualMachineService> logger,
            ApplicationDbContext applicationDbContext,
                                UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// Retorna as VMs que uma empresa informada tem.
        /// </summary>
        /// <param name="idCompanyNullable"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VirtualMachineModel>> GetAsync(ClaimsPrincipal callingUser, int? idCompanyNullable = null, int[] ids = null)
        {

            var idsToFind = ids == null || ids.Length == 0 ? new int[] { } : ids.AsEnumerable();

            bool isAdmin = callingUser.IsInRole(ROLES.ADMINISTRATOR_NAME);

           
            var idUser = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var idCompany = idCompanyNullable ?? 0;

            Console.WriteLine($"isAdmin:{isAdmin}, IdUser:\"{idUser}\"");

            var itens = //from company in applicationDbContext.Companies
                        (from vm in applicationDbContext.VirtualMachines
                         join company in applicationDbContext.Companies on vm.CompanyId equals company.Id
                         from companyUser in company.CompanyUsers.DefaultIfEmpty()
                         where
                         //Caso seja Admin, exibe todas as VMs ou somente as VMs da empresa informada.
                         (isAdmin && (idCompany == 0 || company.Id == idCompany))
                         ||
                         //Caso nao seja Admin, só exibe as VMs que o usuario-chamador tem acesso.
                         (!isAdmin && companyUser != null && companyUser.UserId == idUser)





                         select new VirtualMachineModel
                         {
                             Id = vm.Id,
                             Name = vm.Name,
                             Description = vm.Description,
                             ResourceGroupId = vm.ResourceGroupId,
                             SubscriptionId = vm.SubscriptionId,
                             CompanyId = vm.CompanyId,
                             Cloud = vm.Cloud,
                             VirtualMachineMonitoring = vm.VirtualMachineMonitoring
                         }).Distinct();
                     

            var list = itens.ToList();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"idCompany: {idCompany}, Total: {list.Count}");
            Console.ResetColor();
            return list;
        }


      

        public async Task<ResponseBase<int>> AddAsync(VirtualMachineModel target)
        {
            var response = new ResponseBase<int>();
            if (target == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Null", ErrorMessage = "Nenhuma empresa informada para adicionar" });
                return response;
            }

            if (string.IsNullOrWhiteSpace(target.Name) || target.Name.Length <= 3)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Name", ErrorMessage = "Nome deve ser informado" });
                return response;
            }

            var dbVirtualMachineRow = new VirtualMachineRow
            {
                Name = target.Name,
                Cloud = target.Cloud,
                Description = target.Description,
                ResourceGroupId = target.ResourceGroupId,
                SubscriptionId = target.SubscriptionId,
                CompanyId = target.CompanyId,
                VirtualMachineMonitoring = target.VirtualMachineMonitoring,
                
            };
           

            try
            {
                var ssss = await applicationDbContext.VirtualMachines.AddAsync(dbVirtualMachineRow);
                await applicationDbContext.SaveChangesAsync();

                if (dbVirtualMachineRow.Id == 0)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Erro", ErrorMessage = "Erro desconhecido ao tentar criar nova empresa" });
                    return response;
                }
                Console.WriteLine($"VirtualMachineRow.Id: {dbVirtualMachineRow.Id}  -  VirtualMachineRow.CompanyId: {dbVirtualMachineRow.CompanyId}");

                response.Item = dbVirtualMachineRow.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add("Exception", ex.ToString());
            }

            return response;
        }



        ///// <summary>
        ///// Atualiza os dados da empresa informada..
        ///// </summary>
        ///// <param name="target"></param>
        ///// <returns></returns>
        public async Task<ResponseBase> UpdateAsync(VirtualMachineModel target)
        {
            var response = new ResponseBase();

            if (target == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Null", ErrorMessage = "O objeto informado é nulo" });
                return response;
            }

            try
            {

                foreach (var dbCompany in applicationDbContext.VirtualMachines.Where(f => f.Id == target.Id))
                {
                    dbCompany.Name = target.Name;
                    dbCompany.Cloud = target.Cloud;
                    dbCompany.Description = target.Description;
                    dbCompany.ResourceGroupId = target.ResourceGroupId;
                    dbCompany.SubscriptionId = target.SubscriptionId;
                    dbCompany.VirtualMachineMonitoring = target.VirtualMachineMonitoring;
                }

                await applicationDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Erro", ErrorMessage = "Erro ao atualizar VM" });
                return response;
            }


            response.Success = true;
            return response;
        }




        public async Task<ResponseBase> DeleteAsync(int id)
        {
            var response = new ResponseBase();
            if (id <= 0)
            {
                response.Errors.Add("Null", "Id deve ser informado");
                return response;
            }

            var vm = await applicationDbContext.VirtualMachines.Where(f => f.Id == id).FirstOrDefaultAsync();

            if (vm == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "NotFound", ErrorMessage = "Empresa nao encontrada com o Id informado" });
                return response;
            }

            var result = applicationDbContext.VirtualMachines.Remove(vm);

            await applicationDbContext.SaveChangesAsync();
            response.Success = true;
            return response;
        }



    }
}
