using System.Collections.Generic;
using System.Threading.Tasks;
using Platform.Shared.Models;

namespace Platform.Server.Services
{
    public interface ICompanyService
    {

        Task<ResponseBase<int>> AddAsync(CompanyModel target);
        Task<IEnumerable<CompanyModel>> GetAsync(int[] ids = null);
        Task<ResponseBase> UpdateAsync(CompanyModel target);
        Task<ResponseBase> DeleteAsync(int id);
    }
}
