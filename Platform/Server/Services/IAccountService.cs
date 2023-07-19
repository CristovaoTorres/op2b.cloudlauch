using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    public interface IAccountService
    {
        Task<ResponseBase> DeleteAsync(string Id);

        Task<ResponseBase<string>> AddUserAsync(UserModel newUser);
        Task<ResponseBase> UpdateAsync(UserModel newUser);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
    }
}
