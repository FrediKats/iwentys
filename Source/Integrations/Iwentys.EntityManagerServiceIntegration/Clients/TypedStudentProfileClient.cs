using AutoMapper;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class TypedStudentProfileClient
{
    private readonly StudentProfileClient _client;
    private readonly IMapper _mapper;

    public TypedStudentProfileClient(StudentProfileClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<Student>> GetAsync()
    {
        IReadOnlyCollection<StudentInfoDto> studentInfoDtos = await _client.GetAsync();
        return studentInfoDtos
            .Select(s => _mapper.Map<Student>(s))
            .ToList();
    }

    public async Task<Student> GetByIdAsync(int id)
    {
        StudentInfoDto student = await _client.GetByIdAsync(id);
        return _mapper.Map<Student>(student);
    }

    public async Task<IReadOnlyCollection<Student>> GetByCourseIdAsync(int courseId)
    {
        IReadOnlyCollection<StudentInfoDto> studentInfoDtos = await _client.GetByCourseIdAsync(courseId);
        return studentInfoDtos
            .Select(s => _mapper.Map<Student>(s))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Student>> GetByGroupIdAsync(int groupId)
    {
        IReadOnlyCollection<StudentInfoDto> studentInfoDtos = await _client.GetByGroupIdAsync(groupId);
        return studentInfoDtos
            .Select(s => _mapper.Map<Student>(s))
            .ToList();
    }
}