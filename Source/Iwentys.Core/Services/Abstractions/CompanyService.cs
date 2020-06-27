using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ICompanyService
    {
        Company[] Get();
        Company Get(int id);
    }
}