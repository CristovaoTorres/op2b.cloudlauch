//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Platform.Server.Models
//{
//    public class VirtualMachineRow
//    {

//        [Column(name: "IdVirtualMachine")]
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }


//        public string Name { get; set; }

//        public string Description { get; set; }

//        [Required]
//        public string ResourceGroupId { get; set; }

//        [Required]
//        public string SubscriptionId { get; set; }


//        public int CompanyId { get; set; }

//        /// <summary>
//        /// A empresa pra quem a virtual machine roda.
//        /// </summary>
//        [ForeignKey("CompanyId")]
//        public CompanyRow Company { get; set; }
//    }
//}
