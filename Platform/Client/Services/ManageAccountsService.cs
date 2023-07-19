using Platform.Client.Models;
using Platform.Shared;
using Platform.Shared.Extensions;
using Platform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Client.Services
{
    public class ManageAccountsService
    {
        private readonly HttpClient http;

        public ManageAccountsService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<AggregateResponse<ResponseBase>> DeleteUser(UserModel user)
        {
            var response = new AggregateResponse<ResponseBase>();
         
            var responseMessage = await http.DeleteAsync($"Account?id={user.Id}");
            var innerResponse = await responseMessage.ConvertTo<ResponseBase>();
            response.Responses.Add(innerResponse);
            if (innerResponse.Success)
            {
                response.SuccessMessage = "Usuario excluido com sucesso";
                response.Success = true;
            }
            else
            {
                response.ErrorMessage = innerResponse.ToErrorMessage();
            }
            return response;
        }



        public async Task<AggregateResponse<ResponseBase>> SaveUserSchedule(UserModel user, UserVMScheduling startVMmSchedule, UserVMScheduling stopVMSchedule, IList<AppClaimModel> userClaims)
        {
            var response = new AggregateResponse<ResponseBase>();

            var schedules = new List<UserVMScheduling>();


            startVMmSchedule.StartTime = new DateTime(1900, 1, 1, startVMmSchedule.StartTime.Hour, startVMmSchedule.StartTime.Minute, startVMmSchedule.StartTime.Second);
            stopVMSchedule.StartTime = new DateTime(1900, 1, 1, stopVMSchedule.StartTime.Hour, stopVMSchedule.StartTime.Minute, stopVMSchedule.StartTime.Second);
            schedules.Add(startVMmSchedule);
            schedules.Add(stopVMSchedule);

            if (string.IsNullOrWhiteSpace(user.Id)) //usuario ainda nao foi salvo
            {
                response.ErrorMessage = "Usuario deve existir no banco de dados para atualizar o Schedule";
                return response;
            }

            var innerResponse1 = await http.PostAsJsonAsync<IList<AppClaimModel>, ResponseBase>($"UserClaims?userId={user.Id}", userClaims);
            response.Responses.Add(innerResponse1);

            //if (userClaims.Contains(ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM))
            //{
            //    schedules.Add(UserVMScheduling.CreateEmpty(user.Id, (eVMActionTypes.Start)));
            //}

            //if (userClaims.Contains(ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM))
            //{
            //    schedules.Add(UserVMScheduling.CreateEmpty(user.Id, (eVMActionTypes.Stop)));
            //}

            //if (schedules.Any())
            //{
            //    await this.AddSchedulingsAsync(user.Id, schedules);
            //}

            var innerResponse = await this.AddSchedulingsAsync(user.Id, schedules);
          
                if (innerResponse.Success)
                {
                    response.SuccessMessage = "Dados atualizados com sucesso.";
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = innerResponse.ToErrorMessage();
                    response.Success = false;
                }
   

            return response;
            
        }

        public async Task<AggregateResponse<ResponseBase>> SaveUser(UserModel user, string currentUserRole)
        {
            var response = new AggregateResponse<ResponseBase>();


            var schedules = new List<UserVMScheduling>();
            if (string.IsNullOrWhiteSpace(user.Id)) //adiciona novo usuario
            {
                user.Roles = new List<string> { currentUserRole };

                var innerResponse = await http.PostAsJsonAsync<UserModel, ResponseBase<string>>("Account", user);
                response.Responses.Add(innerResponse);
                if (innerResponse.Success)
                {
                    response.SuccessMessage = "Usuario Adicionado com sucesso";
                    user.Id = innerResponse.Item;

                    //var innerResponse1 = await http.PostAsJsonAsync<IList<AppClaimModel>, ResponseBase>($"UserClaims?userId={user.Id}", userClaims);
                    //response.Responses.Add(innerResponse1);

                    //if (userClaims.Any(f => f.Type.Equals(ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM, StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    schedules.Add(UserVMScheduling.CreateEmpty(user.Id, (eVMActionTypes.Start)));
                    //}

                    //if (userClaims.Any(f => f.Type.Equals(ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM, StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    schedules.Add(UserVMScheduling.CreateEmpty(user.Id, (eVMActionTypes.Stop)));
                    //}

                    //if (schedules.Any())
                    //{
                    //    await this.AddSchedulingsAsync(user.Id, schedules);
                    //}

                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = innerResponse.ToErrorMessage();
                }
            }
            else //atualiza usuario existente
            {
                user.Roles.Clear();
                user.Roles.Add(currentUserRole);

                var responseMessage = await http.PutAsJsonAsync<UserModel>("Account", user);
                var innerResponse = await responseMessage.ConvertTo<ResponseBase>();
                response.Responses.Add(innerResponse);
                if (innerResponse.Success)
                {
                    //var innerResponse1 = await http.PostAsJsonAsync<IList<AppClaimModel>, ResponseBase>($"UserClaims?userId={user.Id}", userClaims);
                    //response.Responses.Add(innerResponse1);
                 
                    response.SuccessMessage = "Usuario atualizado com sucesso";
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = innerResponse.ToErrorMessage();
                }
            }

            return response;
        }



        public async Task<ResponseBase<IList<UserVMScheduling>>> GetSchedulingsAsync(string userId)
        {
            var response = await http.GetFromJsonAsync<ResponseBase<IList<UserVMScheduling>>>($"UserScheduling?userId={userId}");
            return response;
        }

        public async Task<ResponseBase> AddSchedulingsAsync(string userId, IList<UserVMScheduling> items)
        {
            var response = await http.PostAsJsonAsync<IList<UserVMScheduling>, ResponseBase>($"UserScheduling?userId={userId}", items);
            return response;
        }
    }
}
