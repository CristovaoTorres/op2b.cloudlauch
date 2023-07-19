using System.ComponentModel.DataAnnotations;

namespace Platform.Shared.Models
{
    public class VirtualMachineModel
    {
        public VirtualMachineModel()
        {
        }

        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string ResourceGroupId { get; set; }

        [Required]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Nome da virtual machine que deve ser usada para monitorar o status de ligada/desligada no Resource-Group.
        /// Caso seja null, sera assumido que o resource-group tem apenas 1 VM.
        /// 
        /// Aplicavel quando o <see cref="ResourceGroupId"/> tem mais de uma VM dentro dele.
        /// </summary>
        public string VirtualMachineMonitoring { get; set; }

        public string Description { get; set; }
        public int CompanyId { get; set; }
        public int TotalUsers { get; set; }

        public string Cloud { get; set; }

        public eVMPowerState VMState { get; set; }


        public bool CannotStart { get; set; }
        public bool CannotStop { get; set; }
    }

}
