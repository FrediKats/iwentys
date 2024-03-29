﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Newsfeeds;

public static class CreateSubjectNewsfeed
{
    public class Query : IRequest<Response>
    {
        public NewsfeedCreateViewModel CreateViewModel { get; }
        public AuthorizedUser AuthorizedUser { get; }
        public int SubjectId { get; }

        public Query(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
        {
            CreateViewModel = createViewModel;
            AuthorizedUser = authorizedUser;
            SubjectId = subjectId;
        }
    }

    public class Response
    {
        public Response(NewsfeedViewModel newsfeeds)
        {
            Newsfeeds = newsfeeds;
        }

        public NewsfeedViewModel Newsfeeds { get; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser author = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            SubjectNewsfeed newsfeedEntity;
            if (author.CheckIsAdmin(out SystemAdminUser admin))
            {
                newsfeedEntity = SubjectNewsfeed.CreateAsSystemAdmin(request.CreateViewModel, admin, request.SubjectId);
            }
            else
            {
                Student student = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(author.Id);
                if (student.GroupId is null)
                    throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(student.Id);

                StudyGroupProfileResponseDto studyGroup = await _entityManagerApiClient.Client.StudyGroups.GetByStudentIdAsync(author.Id);
                if (studyGroup.GroupAdminId != author.Id)
                    throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(student.Id);

                newsfeedEntity = SubjectNewsfeed.CreateAsGroupAdmin(request.CreateViewModel, student, request.SubjectId);
            }

            _context.SubjectNewsfeeds.Add(newsfeedEntity);

            NewsfeedViewModel result = await _context
                .Newsfeeds
                .Where(n => n.Id == newsfeedEntity.NewsfeedId)
                .Select(NewsfeedViewModel.FromEntity)
                .SingleAsync();

            return new Response(result);
        }
    }
}