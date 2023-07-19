using Newtonsoft.Json;
using Platform.Shared.Models;
using Platform.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Extensions
{

    /// <summary>
    ///  Acesso aos CLAIMS de permissoes que este aplicativo-Blazor tem.
    /// </summary>
    public static class ClaimsPermissionsExtensions
    {

        ///// <summary>
        ///// json que representa o schedule do dia inteiro, todos os dias da semana, permitindo
        ///// que o suuario possa startar/stopar as VMs a qualquer momento do dia.
        ///// </summary>
        //static string json_defaultBehaviourSchedules = null;

        static ClaimsPermissionsExtensions()
        {
           // json_defaultBehaviourSchedules =  PrepareJsonSchedule();
        }

        //private static string PrepareJsonSchedule()
        //{
           
        //    var inicioDia = new TimeSpan( 0, 0, 0);
        //    var finalDia = new TimeSpan(23, 59, 59);


        //    var schedules = new List<UserVMScheduling>();
        //    var weekdays = DateAndTimeExtensions.GetAllWeekDays();

        //    var startScheduling = new UserVMScheduling { ActionType = eVMActionTypes.Start, StartTime = inicioDia, EndTime = finalDia, Days = new List<eDayOfWeekEF>(weekdays) };
        //    var stopScheduling = new UserVMScheduling { ActionType = eVMActionTypes.Stop, StartTime = inicioDia, EndTime = finalDia, Days = new List<eDayOfWeekEF>(weekdays) };

        //    schedules.Add(startScheduling);
        //    schedules.Add(stopScheduling);

        //    //for (int i = 0; i < weekdays.Length; i++)
        //    //{
        //    //    var weekDay = weekdays[i];
        //    //    startScheduling.Days.Add(weekDay);
        //    //    stopScheduling.Days.Add(weekDay);
        //    //}

        //    return JsonConvert.SerializeObject(schedules);
        //}

        /// <summary>
        /// Retorna todos os Claims validos para uso dentro do App.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllValidAppClaimsTypes()
        { 

            return new  string[] { ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM, ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM };
        }

        /// <summary>
        /// Retorna informacoes sobre todos os Claims de permissoes que um usuario participante da role 'Cliente' desse aplicativo pode ter.
        /// </summary>
        /// <returns></returns>
        public static AppClaimModel[] GetClientesClaims()
        {

            return new AppClaimModel[] 
            {
                new AppClaimModel { Type =  ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM, Value = "true", FriendlyDescription = "Iniciar Virtual Machines" },
                new AppClaimModel { Type =  ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM, Value = "true", FriendlyDescription = "Parar Virtual Machines" } 
            };
        }

        /// <summary>
        /// Determina se o claim-type informado existe na relacao de Claims.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static bool Contains(this IList<AppClaimModel> claims, string claimType )
        {
            return claims.Any(f => f.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// Retorna informacoes sobre todos os Claims de permissoes que um usuario participante da role 'Admin' desse aplicativo pode ter.
        /// </summary>
        /// <returns></returns>
        public static AppClaimModel[] GetAdminClaims()
        {
            return new AppClaimModel[] 
            {
                new AppClaimModel { Type = ClaimsConstants.PERMISSION_CLAIM_IS_ADMIN, Value = "true", FriendlyDescription = "Acesso total e irrestrito ao sistema" }
            };
        }

        /// <summary>
        /// Retorna todas as claims de permissoes disponiveis para atribuicao aos usuarios do sistema.
        /// </summary>
        /// <returns></returns>
        public static AppClaimModel[] GetAllClaims()
        {
            return GetClientesClaims().Join(GetAdminClaims());
        }

    }
}
