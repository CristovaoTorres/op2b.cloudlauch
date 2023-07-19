using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {

      

        public string FullName { get; set; }

        public ICollection<CompanyUserRow> CompanyUsers { get; set; }
    }
}
