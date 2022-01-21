﻿using Iwentys.EntityManager.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByCourseId
{
    public record Query(int? CourseId) : IRequest<Response>;
    public record Response(List<GroupProfileResponseDto> Groups);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GroupProfileResponseDto> result = await _context
                .StudyGroups
                .WhereIf(request.CourseId, gs => gs.StudyCourseId == request.CourseId)
                .Select(GroupProfileResponseDto.FromEntity)
                .ToListAsync();

            return new Response(result);
        }
    }
}