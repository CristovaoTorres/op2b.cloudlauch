using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesService(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IEnumerable<RoleModel> GetAllRoles()
        {
            var list = new List<RoleModel>();
            foreach (var x in roleManager.Roles)
            {
                var retUser = new RoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                };
                list.Add(retUser);
            }
            return list;
        }

      

    }
}
