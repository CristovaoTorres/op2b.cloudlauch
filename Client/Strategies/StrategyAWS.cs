using Newtonsoft.Json;
using Platform.Shared.Extensions;
using Platform.Shared.Models;
using Platform.Shared.Models.Externals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Platform.Client.Strategies
{
    public class StrategyAWS : IStrategyCloud
    {
        private readonly HttpClient http;

        public StrategyAWS(HttpClient Http)
        {
            this.http = Http;
        }

        public async Task<eVMPowerState> GetPowerStateAsync(VirtualMachineModel virtualMachine)
        {
            try
            {
                AwsFunctionResponse response = await http.GetFromJsonAsyncExternal<AwsFunctionResponse>("https://q9j6cmpuj8.execute-api.us-east-1.amazonaws.com/default/StartInstanceChekingStatus");

                    // Verificar a propriedade "message"
                    if (response.Message == "running")
                    {
                        return eVMPowerState.Started;
                    }
                    else
                    {
                        return eVMPowerState.Stopped;
                    }
            }
            catch (Exception ex)
            {
                return eVMPowerState.Unkown;
            }
            finally
            {

            }
        }


        public async Task<RemoteVMResponse> StartVMAsync(VirtualMachineModel virtualMachine)
        {
            try
            {
                AwsFunctionResponse response = await http.GetFromJsonAsyncExternal<AwsFunctionResponse>("https://t3un6zbao9.execute-api.us-east-1.amazonaws.com/default/ec2-start-lambda");
                while (true)
                {
                    var powerState = await GetPowerStateAsync(virtualMachine);
                    virtualMachine.VMState = powerState;

                    if (powerState == eVMPowerState.Started)
                    {
                        break;
                    }

                    await Task.Delay(10000); // Aguardar 10 segundos antes da próxima verificação
                }
                return new RemoteVMResponse { Success = true };
  
            }
            catch (Exception ex)
            {
                return new RemoteVMResponse { Success = false };
            }
            finally
            {

            }
        }

        public async Task<RemoteVMResponse> StopVMAsync(VirtualMachineModel virtualMachine)
        {
            try
            {
                AwsFunctionResponse response = await http.GetFromJsonAsyncExternal<AwsFunctionResponse>("https://qlgm933y3d.execute-api.us-east-1.amazonaws.com/default/EC-FTO-StopStack");

                while (true)
                {
                    var powerState = await GetPowerStateAsync(virtualMachine);
                    virtualMachine.VMState = powerState;

                    if (powerState == eVMPowerState.Stopped)
                    {
                        break;
                    }

                    await Task.Delay(10000); // Aguardar 10 segundos antes da próxima verificação
                }
                return new RemoteVMResponse { Success = true };

            }
            catch (Exception ex)
            {
                return new RemoteVMResponse { Success = false };
            }
            finally
            {

            }
        }

        public async Task<bool> TryGetStatusAsync(IList<VirtualMachineModel> virtualMachines)
        {
            var list = new List<Tuple<int, Task<eVMPowerState>>>();

            foreach (var vm in virtualMachines.ToList())
            {
                var task = GetPowerStateAsync(vm);

                var executingStatus = new Tuple<int, Task<eVMPowerState>>(vm.Id, task);//, IdVirtualMachine =  };

                list.Add(executingStatus);
            }

            //aguarda até que todas as tasks tenham finalizado.
            await Task.WhenAll(list.Select(f => f.Item2).ToArray());

            bool refresh = false;
            foreach (var item in list)
            {
                var response = await item.Item2;
                var vm = virtualMachines.FirstOrDefault(f => f.Id == item.Item1);
                if (vm != null)
                {
                    vm.VMState = response;
                    refresh = true;
                }
            }

            return refresh;
        }
    }
}
