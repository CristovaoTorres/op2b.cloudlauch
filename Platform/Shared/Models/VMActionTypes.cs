using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Shared.Models
{
    [Table("VMActionTypes")]
    public class VMActionTypes
    {
        [Column(name: "Id")]
        [Key]
        public eVMActionTypes Type { get; set; }

        public string Name { get; set; }
    }
}
