﻿using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;

namespace Iwentys.EntityManager.WebApi;

public static class GroupSubjectExtensions
{
    public static IQueryable<Subject> SearchSubjects(this IQueryable<GroupSubject> query, SubjectSearchParametersDto searchParametersDto)
    {
        IQueryable<Subject> newQuery = query
            .WhereIf(searchParametersDto.GroupId, gs => gs.StudyGroupId == searchParametersDto.GroupId)
            .WhereIf(searchParametersDto.StudySemester, gs => gs.StudySemester == searchParametersDto.StudySemester)
            .WhereIf(searchParametersDto.SubjectId, gs => gs.SubjectId == searchParametersDto.SubjectId)
            .WhereIf(searchParametersDto.CourseId, gs => gs.StudyGroup.StudyCourseId == searchParametersDto.CourseId)
            .Select(s => s.Subject)
            .Distinct();

        return newQuery;
    }
}