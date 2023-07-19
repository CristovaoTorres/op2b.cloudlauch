using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Shared.Models
{
    public class VirtualMachineRow
    {

        [Column(name: "IdVirtualMachine")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        public string Cloud { get; set; }

        [Required]
        public string ResourceGroupId { get; set; }

        [Required]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Nome da virtual machine que deve ser usada para monitorar o status de ligada/desligada no Resource-Group.
        /// Caso seja null, sera assumido que o resource-group tem apenas 1 VM.
        /// </summary>
        public string VirtualMachineMonitoring { get; set; }

        public int CompanyId { get; set; }

        /// <summary>
        /// A empresa pra quem a virtual machine roda.
        /// </summary>
        [ForeignKey("CompanyId")]
        public CompanyRow Company { get; set; }
    }
}
