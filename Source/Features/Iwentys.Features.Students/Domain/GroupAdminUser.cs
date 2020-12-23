using System;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Domain
{
    public class GroupAdminUser
    {
        public GroupAdminUser(StudentEntity student)
        {
            //TODO: change message
            if (student.GroupId is null)
                throw new Exception();
            
            //TODO: change message
            if (student.Role != UserType.GroupAdmin)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            Student = student;
        }

        public StudentEntity Student { get; }
    }

    public static class GroupAdminUserExtensions
    {
        public static GroupAdminUser EnsureIsGroupAdmin(this StudentEntity profile)
        {
            return new GroupAdminUser(profile);
        }
    }
}