using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    public interface IRolesService
    {
        IEnumerable<RoleModel> GetAllRoles();
    }
}
