using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ICompanyService
    {
        CompanyInfoDto[] Get();
        CompanyInfoDto Get(int id);
    }
}