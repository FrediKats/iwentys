using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Domain
{
    public class GroupName
    {
        public GroupName(string name)
        {
            //FYI: russian letter
            Name = name
                .ToUpper()
                .Substring(0, 5)
                .Replace("М", "M");

            Course = int.Parse(Name.Substring(2, 1));
            Number = int.Parse(Name.Substring(3, 2));
        }

        public int Course { get; }
        public int Number { get; }
        public string Name { get; }

        public async Task<StudyGroupEntity> GetStudyGroup(IGenericRepository<StudyGroupEntity> studyGroupRepository)
        {
            return await studyGroupRepository.GetAsync().FirstOrDefaultAsync(s => s.GroupName == Name);
        }
    }
}