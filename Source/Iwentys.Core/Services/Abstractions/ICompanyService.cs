using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ICompanyService
    {
        CompanyInfoResponse[] Get();
        CompanyInfoResponse Get(int id);

        CompanyWorkRequestDto[] GetCompanyWorkRequest();
        void RequestAdding(int companyId, int userId);
        void ApproveAdding(int userId, int adminId);
    }
}