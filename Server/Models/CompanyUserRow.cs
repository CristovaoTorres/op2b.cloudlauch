//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Platform.Server.Models
//{
//    public class CompanyUserRow
//    {

//        [Column(name: "IdCompanyUser")]
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }



//        public int CompanyId { get; set; }

//        [ForeignKey("CompanyId")]
//        public CompanyRow Company { get; set; }

//        public string UserId { get; set; }

//        [ForeignKey("UserId")]
//        public ApplicationUser User { get; set; }
//    }
//}
