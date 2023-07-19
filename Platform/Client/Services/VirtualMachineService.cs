using Platform.Client.Models;
using Platform.Client.Strategies;
using Platform.Shared.Extensions;
using Platform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Platform.Client.Services
{
    public class VirtualMachineService
    {
        private readonly HttpClient Http;
        private readonly CloudContext cloudContext;
        public event Action RefreshUIRequested;
        public VirtualMachineService(HttpClient http, CloudContext cloudContext)
        {
            this.Http = http;
            this.cloudContext = cloudContext;
        }
        public void RequestRefreshUI()
        {
            RefreshUIRequested?.Invoke();
        }
        public async Task<AggregateResponse<ResponseBase>> StartVMAsync(VirtualMachineModel vm)
        {
            var finalResponse = new AggregateResponse<ResponseBase>();

            vm.VMState = eVMPowerState.Starting;
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var response = await this.cloudContext.StartVMAsync(vm);
            finalResponse.Responses.Add(response);
            if (response != null && response.Success)
            {
                response.Success = true;
                vm.VMState = eVMPowerState.Started;
            }
            else
            {
                response.Success = false;
                vm.VMState = eVMPowerState.Unkown;
            }
            return finalResponse;
        }

        public Task<bool> TryGetStatusAsync(IList<VirtualMachineModel> virtualMachines)
        {
            if (virtualMachines.Count > 0)
            {
                return this.cloudContext.TryGetStatusAsync(virtualMachines);
            }
            else
            {
                return Task.FromResult(false);
            }
        }



        public async Task<AggregateResponse<ResponseBase>> DeleteVirtualMachine(VirtualMachineModel objUser)
        {
            var finalResponse = new AggregateResponse<ResponseBase>();
            var responseMessage = await Http.DeleteAsync($"ManageVirtualMachine?id={objUser.Id}");
            var response = await responseMessage.ConvertTo<ResponseBase>();
            finalResponse.Responses.Add(response);
            if (response.Success)
            {
                finalResponse.Success = true;
                finalResponse.SuccessMessage = "Usuario excluido com sucesso";
            }
            else
            {
                finalResponse.ErrorMessage = response.ToErrorMessage();
            }

            return finalResponse;
        }


        public async Task<AggregateResponse<ResponseBase>> SaveVirtualMachineAsync(VirtualMachineModel vm, int idCompany)
        {
            var finalResponse = new AggregateResponse<ResponseBase>();

            if (vm.Id <= 0) //adiciona novo usuario
            {

                vm.CompanyId = idCompany;
                var responseMessage = await Http.PostAsJsonAsync<VirtualMachineModel>("ManageVirtualMachine", vm);
                var response = await responseMessage.ConvertTo<ResponseBase<int>>();
                finalResponse.Responses.Add(response);
                if (response.Success)
                {
                    finalResponse.Success = true;
                    finalResponse.SuccessMessage = "Usuario Adicionado com sucesso";
                    vm.Id = response.Item;
                }
                else
                {
                    finalResponse.ErrorMessage = response.ToErrorMessage();
                }
            }
            else //atualiza usuario existente
            {

                var responseMessage = await Http.PutAsJsonAsync<VirtualMachineModel>("ManageVirtualMachine", vm);
                var response = await responseMessage.ConvertTo<ResponseBase>();
                if (response.Success)
                {
                    finalResponse.Success = true;
                    finalResponse.SuccessMessage = "alteracao realizada com sucesso";
                }
                else
                {
                    finalResponse.ErrorMessage = response.ToErrorMessage();
                }
            }
            return finalResponse;
        }



        public bool CanExecuteNow(UserVMScheduling schedule)
        {
            if (schedule == null)
            {
                Console.WriteLine($"1 - CanExecuteNow: False");
                return false;
            }
            bool canExecute = false;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (schedule.WeekDaySunday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Monday:
                    if (schedule.WeekDayMonday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (schedule.WeekDayTuesday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (schedule.WeekDayWednesday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (schedule.WeekDayThursday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Friday:
                    if (schedule.WeekDayFriday)
                    {
                        canExecute = true;
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (schedule.WeekDaySaturday)
                    {
                        canExecute = true;
                    }
                    break;
                default:
                    canExecute = false;
                    break;
            }


            if (!canExecute)
            {
                Console.WriteLine($"2 - CanExecuteNow: False");
                return false;
            }

            var now = GetLocalTime().TimeOfDay;

            Console.WriteLine($"3 - CanExecuteNow: -  Now:\"{now}\" StartTime:\"{schedule.StartTime.TimeOfDay}\"  EndTime:\"{schedule.EndTime.TimeOfDay}\"");

            if ((now > schedule.StartTime.TimeOfDay) && (now < schedule.EndTime.TimeOfDay))
            {
                Console.WriteLine($"4 - CanExecuteNow: true  Now:\"{now}\" StartTime:\"{schedule.StartTime.TimeOfDay}\"  EndTime:\"{schedule.EndTime.TimeOfDay}\"");
                return true;
            }

            Console.WriteLine($"5 - CanExecuteNow: False");
            return false;
        }


        private static DateTime GetLocalTime()
        {
            TimeZoneInfo brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Brazil/East");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brazilTimeZone);

            return new DateTime(1900, 1, 1, localTime.Hour, localTime.Minute, localTime.Second, localTime.Millisecond);
        }

        public async Task<AggregateResponse<ResponseBase>> StopVMAsync(VirtualMachineModel vm)
        {
            var finalResponse = new AggregateResponse<ResponseBase>();

            vm.VMState = eVMPowerState.Stopping;
            var response = await this.cloudContext.StopVMAsync(vm);
            finalResponse.Responses.Add(response);

            if (response != null && response.Success)
            {
                finalResponse.Success = true;
                vm.VMState = eVMPowerState.Stopped;
            }
            else
            {
                finalResponse.Success = false;
                vm.VMState = eVMPowerState.Unkown;
            }

            return finalResponse;
        }
    }
}
