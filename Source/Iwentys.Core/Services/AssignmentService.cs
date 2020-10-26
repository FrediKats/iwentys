using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;

namespace Iwentys.Core.Services
{
    public class AssignmentService
    {
        private readonly DatabaseAccessor _database;

        public AssignmentService(DatabaseAccessor database)
        {
            _database = database;
        }

        public AssignmentInfoResponse Create(AuthorizedUser user, AssignmentCreateRequest assignmentCreateRequest)
        {
            return _database.Assignment.Create(user.GetProfile(_database.Student), assignmentCreateRequest).To(AssignmentInfoResponse.Wrap);
        }

        public List<AssignmentInfoResponse> Read(AuthorizedUser user)
        {
            return _database.Assignment
                .Read()
                .Where(a => a.StudentId == user.Id)
                .AsEnumerable()
                .SelectToList(AssignmentInfoResponse.Wrap);
        }
    }
}