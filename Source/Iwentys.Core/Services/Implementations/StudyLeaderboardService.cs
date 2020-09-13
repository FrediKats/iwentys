﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class StudyLeaderboardService : IStudyLeaderboardService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public StudyLeaderboardService(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return _databaseAccessor.GroupSubject.GetSubjectsForDto(searchDto);
        }

        public IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(StudySearchDto searchDto)
        {
            return _databaseAccessor.GroupSubject.GetStudyGroupsForDto(searchDto);
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto)
        {
            if (searchDto.StreamId == null && searchDto.GroupId == null ||
                searchDto.StreamId != null && searchDto.GroupId != null)
            {
                throw new IwentysException("One of StudySearchDto fields: StreamId or GroupId should be null");
            }

            searchDto.StudySemester ??= GetCurrentSemester();

            List<SubjectActivityEntity> result = _databaseAccessor.SubjectActivity.GetStudentActivities(searchDto).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .ToList();
        }

        private static StudySemester GetCurrentSemester()
        {
            //TODO: hack
            return StudySemester.Y19H2;
        }
    }
}
