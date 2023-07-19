using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Platform.Client.Models;
using Platform.Shared.Extensions;
using Platform.Shared.Models;
using Platform.Shared.Models.Externals;

namespace Platform.Client.Strategies
{
    public class StrategyAZURE : IStrategyCloud
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly HttpClient http;
        private readonly AppConfiguration appConfiguration;

        public StrategyAZURE(HttpClient Http, AppConfiguration appConfiguration)
        {
            this.http = Http;
            this.appConfiguration = appConfiguration;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId">Azure Subscription Id</param>
        /// <param name="resourceGroup">Resource Group onde deseja fazer acoes nas VMs</param>
        /// <param name="virtualMachineName">Nome da maquina virtual de onde deseja obter o status. Caso seja omitido, o status sera obtido da primeira-vm existente no Resource-Group</param>
        /// <param name="action">Acao que deseja fazer. As opcoes sao "Get", "Start", "Stop" </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AzureFunctionResponse> RunAsync(VirtualMachineModel virtualMachine, string action)
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            CancellationToken cancellationToken = cts.Token;

            var request = new AzureFunctionRequest
            {
                Resourcegroup = virtualMachine.ResourceGroupId,
                SubscriptionId = virtualMachine.SubscriptionId,
                Action = action,
                Tenantid = "TENANTID",
            };

            var maxRetryAttempt = 30;
            bool shouldRetry = true;
            AzureFunctionResponse response = null;

            try
            {
                var attempt = 0;
                while (shouldRetry)
                {
                    try
                    {
                        attempt++;
                        response = await http.PostAsJsonAsync<AzureFunctionRequest, AzureFunctionResponse>(appConfiguration.VirtualMachineManagementUrl, request, cancellationToken);
                        shouldRetry = false;
                        return response;
                    }
                    catch (OperationCanceledException ex)
                    {
                        Console.WriteLine($"Exception de cancelamento detectada!");
                        maxRetryAttempt--;
                        if (maxRetryAttempt <= 0)
                        {
                            throw;
                        }
                        else
                        {
                            //tenta novamente a partir de proxima iteracao do loop while...
                            Console.WriteLine($"Timeout atingido, tentando novamente executar. Tentativa {attempt} de {maxRetryAttempt}...");
                            await Task.Delay(TimeSpan.FromSeconds(10));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception detectada: {ex.ToString()}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception ao fazer chamada remota do gerenciaor de VMs do Azure: {ex.ToString()}");
                response = new AzureFunctionResponse();
                response.Error = ex.ToString();
                response.PowerState = "Exception";
                response.Status = "Exception";
            }
            return response;
        }

        /// <summary>
        /// Retorna o PowerState para a virtual-machine informada.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="resourceGroup"></param>
        /// <param name="virtualMachineName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<AzureFunctionResponse> GetVirtualMachinePowerStateAsync(VirtualMachineModel virtualMachine)
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            CancellationToken cancellationToken = cts.Token;

            var request = new AzureFunctionRequest
            {
                Resourcegroup = virtualMachine.ResourceGroupId,
                SubscriptionId = virtualMachine.SubscriptionId,
                Action = "get",
                Tenantid = "TENANTID",
            };

            var maxRetryAttempt = 30;
            bool shouldRetry = true;
            AzureFunctionResponse response = null;

            try
            {
                var attempt = 0;
                while (shouldRetry)
                {
                    try
                    {
                        attempt++;

                        if (string.IsNullOrEmpty(virtualMachine.VirtualMachineMonitoring))
                        {
                            var response1 = await http.PostAsJsonAsync<AzureFunctionRequest, AzureFunctionResponse>(appConfiguration.VirtualMachineManagementUrl, request, cancellationToken);
                            shouldRetry = false;
                            return response1;
                        }
                        else
                        {
                            var response1 = await http.PostAsJsonAsync<AzureFunctionRequest, List<AzureFunctionVirtualMachineStateResponse>>(appConfiguration.VirtualMachineManagementUrl, request, cancellationToken);

                            var responseVirtualMachine = response1.FirstOrDefault(f => f.Name.Equals(virtualMachine.VirtualMachineMonitoring, StringComparison.OrdinalIgnoreCase));
                            shouldRetry = false;
                            if (responseVirtualMachine == null)
                            {
                                return new AzureFunctionResponse();
                            }
                            else
                            {
                                return new AzureFunctionResponse { Name = responseVirtualMachine.Name, PowerState = responseVirtualMachine.PowerState };
                            }
                        }

                    }
                    catch (OperationCanceledException ex)
                    {
                        Console.WriteLine($"Exception de cancelamento detectada!");
                        maxRetryAttempt--;
                        if (maxRetryAttempt <= 0)
                        {
                            throw;
                        }
                        else
                        {
                            //tenta novamente a partir de proxima iteracao do loop while...
                            Console.WriteLine($"Timeout atingido, tentando novamente executar. Tentativa {attempt} de {maxRetryAttempt}...");
                            await Task.Delay(TimeSpan.FromSeconds(10));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception detectada: {ex.ToString()}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception ao fazer chamada remota do gerenciaor de VMs do Azure: {ex.ToString()}");
                response = new AzureFunctionResponse();
                response.Error = ex.ToString();
                response.PowerState = "Exception";
                response.Status = "Exception";
            }
            return response;
        }


        public async Task<RemoteVMResponse> StartVMAsync(VirtualMachineModel virtualMachine)
        {
            var response = await RunAsync(virtualMachine, "start");

            if (response != null && response.Status.Equals("Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                return new RemoteVMResponse { Success = true };
            }
            else
            {
                return new RemoteVMResponse { Success = false };
            }
        }

        public async Task<RemoteVMResponse> StopVMAsync(VirtualMachineModel virtualMachine)
        {
            var response = await RunAsync(virtualMachine, "stop");

            if (response != null && response.Status.Equals("Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                return new RemoteVMResponse { Success = true };
            }
            else
            {
                return new RemoteVMResponse { Success = false };
            }
        }

        public async Task<eVMPowerState> GetPowerStateAsync(VirtualMachineModel virtualMachine)
        {
            if (string.IsNullOrWhiteSpace(virtualMachine.VirtualMachineMonitoring))
            {
                virtualMachine.VirtualMachineMonitoring = string.Empty;
            }
            var response = await GetVirtualMachinePowerStateAsync(virtualMachine);

            if (string.IsNullOrEmpty(response.PowerState))
            {
                response.PowerState = response.DisplayStatus;
            }

            if (response.PowerState.Equals("VM deallocated", StringComparison.OrdinalIgnoreCase)) //stopada
            {
                return eVMPowerState.Stopped; //permito o botao START na UI
            }
            else if (response.PowerState.Equals("VM running", StringComparison.OrdinalIgnoreCase) || response.PowerState.Equals("VM stopped", StringComparison.OrdinalIgnoreCase)) //vm rodando...
            {
                return eVMPowerState.Started; //Permito o botao "STOP" na UI
            }

            return eVMPowerState.Unkown;
        }

    }
}
