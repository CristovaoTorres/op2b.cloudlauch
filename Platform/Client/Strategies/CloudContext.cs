using Platform.Shared.Models;
using Platform.Shared.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Client.Strategies
{
    public class CloudContext
    {
        private IStrategyCloud strategy;

        private readonly IStrategyCloudFactory strategyFactory;

        public CloudContext(IStrategyCloudFactory strategyFactory)
        {
            this.strategyFactory = strategyFactory;
            this.strategy = null; // Defina uma estratégia padrão ou null, conforme necessário
        }


        private void SetStrategy(VirtualMachineModel virtualMachine)
        {
            this.strategy = strategyFactory.CreateStrategy(virtualMachine.Cloud);
        }

        public async Task<bool> TryGetStatusAsync(IList<VirtualMachineModel> virtualMachines)
        {

            List<VirtualMachineModel> azureVMs = new List<VirtualMachineModel>();
            List<VirtualMachineModel> awsVMs = new List<VirtualMachineModel>();

            foreach (var vm in virtualMachines)
            {
                if (vm.Cloud == eCloudProvider.Azure.GetStringValue())
                {
                    azureVMs.Add(vm);
                }
                else if (vm.Cloud == eCloudProvider.AWS.GetStringValue())
                {
                    awsVMs.Add(vm);
                }
                // Adicione outros blocos "else if" para lidar com diferentes valores da propriedade "Cloud", se necessário.
            }

            if (virtualMachines.Count > 0)
            {
                bool azureStatus = false;
                bool awsStatus = false;

                if (azureVMs.Count > 0)
                {
                    VirtualMachineModel firstVirtualMachine = azureVMs[0];
                    SetStrategy(firstVirtualMachine);
                    azureStatus = await strategy.TryGetStatusAsync(azureVMs);
                }

                if (awsVMs.Count > 0)
                {
                    VirtualMachineModel firstVirtualMachine = awsVMs[0];
                    SetStrategy(firstVirtualMachine);
                    awsStatus = await strategy.TryGetStatusAsync(awsVMs);
                }

                return azureStatus | awsStatus;

            }
            else
            {
                return false;
            }
        }

        public async Task<eVMPowerState> GetPowerStateAsync(VirtualMachineModel virtualMachine)
        {
            SetStrategy(virtualMachine);
            return await strategy.GetPowerStateAsync(virtualMachine);
        }

        public async Task<RemoteVMResponse> StartVMAsync(VirtualMachineModel virtualMachine)
        {
            SetStrategy(virtualMachine);
            return await strategy.StartVMAsync(virtualMachine);
        }

        public async Task<RemoteVMResponse> StopVMAsync(VirtualMachineModel virtualMachine)
        {
            SetStrategy(virtualMachine);
            return await strategy.StopVMAsync(virtualMachine);
        }
    }
}
