//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//

//namespace Platform.Server.Models
//{

//    public class CompanyRow
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//        public string Name { get; set; }

//        /// <summary>
//        /// Relacao de usuarios que fazem parte  desta <see cref="CompanyRow"/>.
//        /// </summary>
//        public  ICollection<CompanyUserRow> CompanyUsers { get; set; }

//        public virtual ICollection<VirtualMachineRow> VirtualMachines { get; set; }

//    }
//}
