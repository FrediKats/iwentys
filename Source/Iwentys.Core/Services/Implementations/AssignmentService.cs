using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;

namespace Iwentys.Core.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly DatabaseAccessor _database;

        public AssignmentService(DatabaseAccessor database)
        {
            _database = database;
        }

        public AssignmentInfoDto Create(AuthorizedUser user, AssignmentCreateDto assignmentCreateDto)
        {
            return _database.Assignment.Create(user.GetProfile(_database.Student), assignmentCreateDto).To(AssignmentInfoDto.Wrap);
        }

        public List<AssignmentInfoDto> Read(AuthorizedUser user)
        {
            return _database.Assignment
                .Read()
                .Where(a => a.StudentId == user.Id)
                .AsEnumerable()
                .SelectToList(AssignmentInfoDto.Wrap);
        }
    }
}