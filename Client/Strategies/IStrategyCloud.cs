using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Platform.Shared.Models;

namespace Platform.Client.Strategies
{
    public interface IStrategyCloud
    {
        /// <summary>
        /// Tenta Obter o status para as maquinas virtuais informadas.
        /// </summary>
        /// <param name="virtualMachines"></param>
        /// <returns></returns>
        Task<bool> TryGetStatusAsync(IList<VirtualMachineModel> virtualMachines);

        /// <summary>
        /// Retorna o <see cref="eVMPowerState"/> da VM que roda na virtual machine informada,
        /// dependendo da implementacao sera necess'ario adicionar novos atributos ao VirtualMachineModel.
        /// </summary>
        /// <param name="virtualMachine"></param>
        /// <returns></returns>
        Task<eVMPowerState> GetPowerStateAsync(VirtualMachineModel virtualMachine);

        /// <summary>
        /// Inicia a execucao da VM que roda no virtual machine informado.
        /// </summary>
        /// <param name="virtualMachine"></param>
        /// <returns></returns>
        Task<RemoteVMResponse> StartVMAsync(VirtualMachineModel virtualMachine);

        /// <summary>
        /// Interrompe a execucao da VM que roda novirtual machine informado.
        /// </summary>
        /// <param name="virtualMachine"></param>
        /// <returns></returns>
        Task<RemoteVMResponse> StopVMAsync(VirtualMachineModel virtualMachine);
    }
}
