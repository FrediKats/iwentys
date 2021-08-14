﻿using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetSubjectById
    {
        public class Query : IRequest<Response>
        {
            public int SubjectId { get; set; }

            public Query(int subjectId)
            {
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public Response(SubjectProfileDto subject)
            {
                Subject = subject;
            }

            public SubjectProfileDto Subject { get; set; }
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
                SubjectProfileDto result = await _context
                    .Subjects
                    .FirstAsync(s => s.Id == request.SubjectId)
                    .To(entity => new SubjectProfileDto(entity));

                return new Response(result);
            }
        }
    }
}