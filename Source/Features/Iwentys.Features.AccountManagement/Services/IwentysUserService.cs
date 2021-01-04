using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.AccountManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.AccountManagement.Services
{
    public class IwentysUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;

        public IwentysUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public Task<IwentysUserInfoDto> Get(int id)
        {
            return _userRepository
                .Get()
                .Where(u => u.Id == id)
                .Select(u => new IwentysUserInfoDto(u))
                .SingleAsync();
        }
    }
}