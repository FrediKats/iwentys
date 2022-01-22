using AutoMapper;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class TypedIwentysUserProfileClient
{
    private readonly IwentysUserProfileClient _client;
    private readonly IMapper _mapper;

    public TypedIwentysUserProfileClient(IwentysUserProfileClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<IwentysUser>> GetAsync()
    {
        IReadOnlyCollection<IwentysUserInfoDto> studentInfoDtos = await _client.GetAsync();
        return studentInfoDtos
            .Select(s => _mapper.Map<IwentysUser>(s))
            .ToList();
    }

    public async Task<IwentysUser> GetByIdAsync(int id)
    {
        IwentysUserInfoDto user = await _client.GetByIdAsync(id);
        return _mapper.Map<IwentysUser>(user);
    }

    public void Update(IwentysUser user)
    {
        //TODO: move github update command to EntityManager service
        throw new InnerLogicException("User update command is not available.");
    }
}