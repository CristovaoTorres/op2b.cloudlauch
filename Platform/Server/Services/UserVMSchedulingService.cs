using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Platform.Server.Data;
using Platform.Shared;
using Platform.Shared.Extensions;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    /// <summary>
    /// Servico para controlar quando as VMs podem ser ligadas/desligadas pelos usuarios que possuirem os claims
    /// <see cref="Platform.Shared.ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM"/> e/ou 
    /// <see cref="Platform.Shared.ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM"/>
    /// </summary>
    public class UserVMSchedulingService : IUserVMSchedulingService
    {
        private readonly ILogger<UserVMSchedulingService> logger;
       // private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceProvider serviceProvider;

        public UserVMSchedulingService(ILogger<UserVMSchedulingService> logger,
                                UserManager<ApplicationUser> userManager,
                                IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            this.logger = logger;
           // this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.serviceProvider = serviceProvider;
        }


        public async Task<ResponseBase<IList<UserVMScheduling>>> GetAsync(ClaimsPrincipal caller, string userId)
        {

            ResponseBase<IList<UserVMScheduling>> response = new ResponseBase<IList<UserVMScheduling>>();
            response.Item = new List<UserVMScheduling>();

            bool isAllowed = caller.IsAllowed(userId);

            if (!isAllowed)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Denied", ErrorMessage = "Permissao de acesso negada" });
                logger.LogError("Permissao de acesso negada");
                return response;
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "NotFound", ErrorMessage = $"O usuario informado nao foi encontrado" });
                logger.LogError($"O usuario \"{userId}\" nao foi encontrado.");
                return response;
            }


            using (var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


                    var userSchedulings = await applicationDbContext.UserVMSchedulings.Where(f => f.UserId == userId).ToListAsync();

                    response.Item = userSchedulings;

                    response.Success = userSchedulings.Any();

                    //var startStopScheduleClaimsFound = applicationDbContext.UserClaims.Any(claim => claim.UserId == userId && (ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM == claim.ClaimType || ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM == claim.ClaimType) && claim.ClaimValue == "true");

                    //List<UserVMScheduling> userSchedulings = null;
                    //if (startStopScheduleClaimsFound)
                    //{

                    //}
                    //else
                    //{
                    //    logger.LogWarning($"Nenhum Claim Type \"{ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM}\" ou \"{ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM}\" encontrado para o usuario \"{userId}\" ");
                    //}


                }
                catch (System.Exception ex)
                {
                    logger.LogError("Erro ao obter dados scheduling de VMs",ex);
                    throw;
                }

            }


          
            return response;
        }

        public async Task<ResponseBase> AddAsync(ClaimsPrincipal caller, string userId, IList<UserVMScheduling> schedules, bool removePreviousSchedules = true)
        {
            ResponseBase response = new ResponseBase();
            bool isAllowed = caller.IsAllowed(userId);

            if (!isAllowed)
            {
                response.ValidationResults.Add(new ValidationFailure { ErrorCode = "Denied", ErrorMessage = "Permissao de acesso negada" });
                logger.LogError("Permissao de acesso negada");
                return response;
            }
            var appUserClaims = new List<AppClaimModel>();


            using (var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (removePreviousSchedules)
                    {
                        var obsoleteItens = (await GetAsync(caller, userId)).Item;

                        if (obsoleteItens.Any())
                        {
                            applicationDbContext.UserVMSchedulings.RemoveRange(obsoleteItens);
                            await applicationDbContext.SaveChangesAsync();
                        }
                    }

                    if (!schedules.Any())
                    {
                        response.Success = true;
                        return response;
                    }

                    //Salva os Schedules informados dentro dos Claims que representam permissoes de Ligar/Dsligar as VMs
                    foreach (var schedule in schedules)
                    {
                        schedule.UserId = userId;
                        applicationDbContext.UserVMSchedulings.Add(schedule);
                    }

                    if (await applicationDbContext.SaveChangesAsync() > 0)
                    {
                        response.Success = true;
                    }

                }
                catch (System.Exception ex)
                {
                    logger.LogError("Erro ao adicionar dados scheduling de VMs", ex);
                    throw;
                }

            }
          
           
            return response;
        }


    }


}
