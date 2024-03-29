﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Study;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification;

public static class CourseRatingForceRefresh
{
    public class Query : IRequest<Response>
    {
        public Query(int courseId)
        {
            CourseId = courseId;
        }

        public int CourseId { get; set; }
    }

    public class Response
    {
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;

        public Handler(IwentysDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<CourseLeaderboardRow> oldRows = await _context
                .CourseLeaderboardRows
                .Where(clr => clr.CourseId == request.CourseId)
                .ToListAsync();

            IReadOnlyCollection<SubjectActivity> result = await _context
                .GetStudentActivities(new StudySearchParametersDto { CourseId = request.CourseId });

            List<CourseLeaderboardRow> newRows = Create(request.CourseId, result.ToList(), oldRows);

            _context.CourseLeaderboardRows.RemoveRange(oldRows);
            _context.CourseLeaderboardRows.AddRange(newRows);
            return new Response();
        }

        public static List<CourseLeaderboardRow> Create(int courseId, List<SubjectActivity> rows, List<CourseLeaderboardRow> oldRows)
        {
            Dictionary<int, int> mapToOld = oldRows.ToDictionary(v => v.StudentId, v => v.Position);

            return rows
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDtoWithoutStudent(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Take(50)
                .OrderByDescending(r => r.Activity)
                .Select((r, position) => CreateRow(r, courseId, position, mapToOld))
                .ToList();
        }

        private static CourseLeaderboardRow CreateRow(StudyLeaderboardRowDtoWithoutStudent row, int courseId, int position, Dictionary<int, int> mapToOld)
        {
            int? oldPosition = null;
            if (mapToOld.TryGetValue(row.StudentId, out var value))
                oldPosition = value;

            return new CourseLeaderboardRow { Position = position + 1, CourseId = courseId, StudentId = row.StudentId, OldPosition = oldPosition };
        }
    }
}