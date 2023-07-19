using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Server.Data;

using Platform.Shared;
using Platform.Shared.Extensions;
using Platform.Shared.Models;

namespace Platform.Server.Services
{

    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public CompanyService(ILogger<CompanyService> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<IEnumerable<CompanyModel>> GetAsync([FromQuery] int[] ids = null)
        {

            var idsToFind = ids == null || ids.Length == 0 ? null : ids.AsEnumerable();


            var itens = from company in applicationDbContext.Companies
                        where idsToFind == null || idsToFind.Contains(company.Id)
                        select new CompanyModel
                        {
                            Id = company.Id,
                            Name = company.Name,
                            TotalVMs = company.VirtualMachines.Count,
                            Users = company.CompanyUsers.Select(f => new UserModel
                            {
                                Id = f.User.Id,
                                Email = f.User.Email,
                                FullName = f.User.FullName,
                                UserName = f.User.UserName,
                                
                            })
                        };

           
            var list = itens.ToList();
            Console.WriteLine($"Total: {itens.Count()}");
            return list;
        }


        public async Task<ResponseBase<int>> AddAsync(CompanyModel target)
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

            var dbCompany = new CompanyRow
            {
                Name = target.Name,
            };

            await AssignUsersToCompany(target.Users.ToArray(), dbCompany);

            try
            {
                var ssss = await applicationDbContext.Companies.AddAsync(dbCompany);
                await applicationDbContext.SaveChangesAsync();

                if (dbCompany.Id == 0)
                {
                    response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Erro", ErrorMessage = "Erro desconhecido ao tentar criar nova empresa" });
                    return response;
                }

                response.Item = dbCompany.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add("Exception", ex.ToString());
            }

            return response;
        }

        private async Task AssignUsersToCompany(UserModel[] companyUsersCandidates, CompanyRow dbCompany)
        {

            var idUsersCandidates = companyUsersCandidates.Select(f => f.Id).AsEnumerable();

            var dbUsers = applicationDbContext.Users.Where(f => idUsersCandidates.Contains(f.Id));

            if (dbCompany.CompanyUsers == null)
            {
                dbCompany.CompanyUsers = new List<CompanyUserRow>();
            }

            var usersToBeRemoved = applicationDbContext.CompanyUsers.Where(f => f.CompanyId == dbCompany.Id);
            applicationDbContext.CompanyUsers.RemoveRange(usersToBeRemoved);

            foreach (var user in dbUsers.ToList())
            {
                dbCompany.CompanyUsers.Add(new CompanyUserRow { Company = dbCompany, User = user });
                Console.WriteLine($"Vinculando \"{user.FullName}\" à \"{dbCompany.Name}\"");

            }


        }


        ///// <summary>
        ///// Atualiza os dados da empresa informada..
        ///// </summary>
        ///// <param name="target"></param>
        ///// <returns></returns>
        public async Task<ResponseBase> UpdateAsync(CompanyModel companyModel)
        {
            var response = new ResponseBase();

            if (companyModel == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Null", ErrorMessage = "Nenhuma empresa informado" });
                return response;
            }

            try
            {

                foreach (var dbCompany in applicationDbContext.Companies.Where(f => f.Id == companyModel.Id).ToList())
                {
                    dbCompany.Name = companyModel.Name;

                    await AssignUsersToCompany(companyModel.Users.ToArray(), dbCompany);
                }

                await applicationDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Erro", ErrorMessage = "Erro ao atualizar empresa" });
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

            var user = await applicationDbContext.Companies.Where(f => f.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "NotFound", ErrorMessage = "Empresa nao encontrada com o Id informado" });
                return response;
            }

            var result = applicationDbContext.Companies.Remove(user);

            await applicationDbContext.SaveChangesAsync();
            response.Success = true;
            return response;
        }



    }
}
