using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ICompanyService
    {
        CompanyInfoDto[] Get();
        CompanyInfoDto Get(int id);

        CompanyWorkRequestDto[] GetCompanyWorkRequest();
        void RequestAdding(int companyId, int userId);
        void ApproveAdding(int userId, int adminId);
    }
}