using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Repositories
{
    //TODO: awful hack
    public static class StudyGroupRepositoryExtensions
    {
        public static async Task<List<GroupProfileResponseDto>> WithStudents(this IQueryable<GroupProfileResponseDto> query, IGenericRepository<Student> studentRepository)
        {
            List<GroupProfileResponseDto> groups = await query.ToListAsync();
            groups.ForEach(g =>
            {
                g.Students = studentRepository.Get().Where(s => s.GroupId == g.Id).Select(s => new StudentInfoDto(s)).ToList();
            });
            return groups;
        }
    }
}