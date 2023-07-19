using System.Collections.Generic;

namespace Platform.Shared.Models
{
    public class CompanyModel
    {
        public CompanyModel()
        {
            Users = new List<UserModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Lista com os Ids dos usuarios que fazem parte da empresa.
        /// </summary>
        public IEnumerable<UserModel> Users { get; set; }
        public int TotalVMs { get; set; }
    }
}
